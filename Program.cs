using System;
using System.Linq;
using marketmanagement.data;
using marketmanagement.datastructures;

using var db = new marketdbcontext();
var trackingtree = new producttree();

Console.WriteLine("supermarket");

if (!db.products.Any())
{
    var samplecategory = new marketmanagement.models.category { name = "Groceries" };
    var samplesupplier = new marketmanagement.models.supplier { name = "Main Distributor", contactinfo = "07771697384" };

    db.categories.Add(samplecategory);
    db.suppliers.Add(samplesupplier);
    db.SaveChanges();

    var sampleproduct = new marketmanagement.models.product
    {
        title = "milk 2L",
        barcode = "50123456",
        price = 2.20m,
        quantityinstock = 45,
        expiryorrestockdate = DateTime.Now.AddDays(7),
        categoryid = samplecategory.id,
        supplierid = samplesupplier.id
    };

    db.products.Add(sampleproduct);
    db.SaveChanges();
    Console.WriteLine("Sample added");
}

var allproducts = db.products.ToList();
foreach (var p in allproducts)
{
    trackingtree.insertproduct(p);
}
Console.WriteLine($"Loaded {allproducts.Count} items in treecache\n");

var checkoutcart = new System.Collections.Generic.List<marketmanagement.models.product>();
bool running = true;

while (running)
{
    Console.WriteLine("retail management control");
    Console.WriteLine("1 scan or type barcode");
    Console.WriteLine("2 view cart and checkout");
    Console.WriteLine("3 search product by name");
    Console.WriteLine("4 inventory reports");
    Console.WriteLine("5 product crud(add/edit/delete)");
    Console.WriteLine("6 supplier registry");
    Console.WriteLine("7 quick restock manager");
    Console.WriteLine("8 cancel cart and exit");
    Console.Write("select an option: ");
    string choice = Console.ReadLine();
    if (choice == "1")
    {
        Console.Write("scan or type item barcode");
        string scaninput = Console.ReadLine();
        var founditem = trackingtree.searchbybarcode(scaninput);

        if (founditem != null)
        {
            int qtyToBuy = 0;
            while (true)
            {
                Console.Write($"How many units of '{founditem.title}' to add? (Available {founditem.quantityinstock}): ");
                if (int.TryParse(Console.ReadLine(), out qtyToBuy) && qtyToBuy > 0)
                {
                    break;
                }
                Console.WriteLine("error  enter a positive number greater than 0.");
            }

          
            if (founditem.quantityinstock >= qtyToBuy)
            {
             
                for (int i = 0; i < qtyToBuy; i++)
                {
                    checkoutcart.Add(founditem);
                }

                founditem.quantityinstock -= qtyToBuy;
                Console.WriteLine($"\n {qtyToBuy}x {founditem.title} added to cart. (Total: £{founditem.price * qtyToBuy})\n");
            }
            else
            {
                Console.WriteLine($"\n (out of stock)   You cannot buy {qtyToBuy} units.\n");
            }
        }
        else
        {
            Console.WriteLine("\nerror barcode could not be resolved.\n");
        }
    }
    else if (choice == "2")
    {
        if (checkoutcart.Count == 0) { Console.WriteLine("\nyour cart is empty\n"); continue; }
        decimal balance = checkoutcart.Sum(item => item.price);
        Console.WriteLine($"\ntotal value £{balance}");
        Console.Write("confirm purchase?(yes/no)");
        string confirm = Console.ReadLine()?.ToLower();

        if (confirm == "yes")
        {
            var newsale = new marketmanagement.models.sale { Transsactiondate = DateTime.Now, totalamount = balance };
            db.sales.Add(newsale);
            db.SaveChanges();

            foreach (var cartitem in checkoutcart)
            {
                var dbrecord = db.products.FirstOrDefault(p => p.id == cartitem.id);
                if (dbrecord != null) dbrecord.quantityinstock = cartitem.quantityinstock;

                db.saleitems.Add(new marketmanagement.models.saleitem
                {
                    saleid = newsale.Id,
                    productid = cartitem.id,
                    quantity = 1,
                    unitprice = cartitem.price
                });
            }
            db.SaveChanges();
            checkoutcart.Clear();
            Console.WriteLine("\n transaction completed\n");
        }
    }
    else if (choice == "3")
    {
        Console.Write("enter item name to search");
        trackingtree.searchbynamepartial(Console.ReadLine());
    }
    else if (choice == "4")
    {
        trackingtree.generatemanagementreports(stockthreshold: 50);

        Console.WriteLine(" sales log");
        var historicalsales = db.sales.ToList();
        if (!historicalsales.Any())
        {
            Console.WriteLine("no transactions recorded yet\n");
        }
        else
        {
            foreach (var s in historicalsales)
            {
                Console.WriteLine($"Sale ID: {s.Id} | Date: {s.Transsactiondate:yyyy-MM-dd HH:mm} | Total Revenue: £{s.totalamount}");
            }
            Console.WriteLine();
        }
    }
    else if (choice == "5")
    {
        Console.WriteLine("\nproduct crud");
        Console.WriteLine("1 create (Add new product)");
        Console.WriteLine("2 updatee (Edit product)");
        Console.WriteLine("3 delete (Remove product)");
        Console.Write("Choose operation: ");
        string crudChoice = Console.ReadLine()?.ToUpper();

        if (crudChoice == "1")
        {
           
            string bc = "";
            while (true)
            {
                Console.Write("Enter Barcode: ");
                bc = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(bc))
                {
                    Console.WriteLine("Barcode field is empty");
                }
                else if (trackingtree.searchbybarcode(bc) != null)
                {
                    Console.WriteLine("This item already exists");
                }
                else
                {
                    break;
                }
            }

            Console.Write("Enter Name: ");
            string name = Console.ReadLine();
            decimal pr = 0;
            while (true)
            {
                Console.Write("Enter Price: ");
                if (decimal.TryParse(Console.ReadLine(), out pr) && pr > 0)
                {
                    break;
                }
                Console.WriteLine("error price must be anumeric value greater than 0");
            }

            int st = 0;
            while (true)
            {
                Console.Write("Enter Stock Quantity: ");
                if (int.TryParse(Console.ReadLine(), out st) && st >= 0)
                {
                    break;
                }
                Console.WriteLine("Error stock levels cannot be negative.");
            }

            var defaultCategory = db.categories.FirstOrDefault();
            var defaultSupplier = db.suppliers.FirstOrDefault();

            if (defaultSupplier == null || defaultCategory == null)
            {
                Console.WriteLine("Error failed Please register a supplier first via option 6");
                continue;
            }

            var newP = new marketmanagement.models.product
            {
                barcode = bc,
                title = name,
                price = pr,
                quantityinstock = st,
                expiryorrestockdate = DateTime.Now.AddDays(14),
                categoryid = defaultCategory.id,
                supplierid = defaultSupplier.id
            };

            db.products.Add(newP);
            db.SaveChanges();
            trackingtree.insertproduct(newP);
            Console.WriteLine($"\n Sucess '{name}' passed all validation and saved\n");
        }
        else if (crudChoice == "2")
        {
            Console.Write("Enter Barcode of item to update: ");
            string bc = Console.ReadLine();
            var dbProd = db.products.FirstOrDefault(p => p.barcode == bc);

            if (dbProd != null)
            {
                Console.Write($"New Name ({dbProd.title}): ");
                string name = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(name)) name = dbProd.title;

                decimal pr = 0;
                while (true)
                {
                    Console.Write($"New Price (£{dbProd.price}): ");
                    if (decimal.TryParse(Console.ReadLine(), out pr) && pr > 0)
                    {
                        break;
                    }
                    Console.WriteLine(" Must be a numeric value greater than 0");
                }

               
                int st = 0;
                while (true)
                {
                    Console.Write($"New Stock ({dbProd.quantityinstock}): ");
                    if (int.TryParse(Console.ReadLine(),out st) && st >= 0)
                    {
                        break;
                    }
                    Console.WriteLine("Stock levels cannot be negative");
                }

                dbProd.title = name;
                dbProd.price = pr;
                dbProd.quantityinstock = st;
                db.SaveChanges();

                trackingtree.UpdateProductInTree(bc, name, pr, st);
                Console.WriteLine("\nDonee Product updated in both storage layers\n");
            }
            else Console.WriteLine("Product not found.");
        }
        else if (crudChoice == "3")
        {
            Console.Write("Enter Barcode of item to delete");string bc = Console.ReadLine();
            var dbProd = db.products.FirstOrDefault(p => p.barcode == bc);
            if (dbProd != null)
            {
                db.products.Remove(dbProd);
                db.SaveChanges();

                trackingtree.DeleteProduct(bc);
                Console.WriteLine("\nremoved from memory and database c\n");
            }
            else Console.WriteLine("Product not found.");
        }
    }
    else if (choice == "6")
    {
        Console.WriteLine("\nsupplier registry");
        Console.Write("Enter Supplier Name: ");
        string sName = Console.ReadLine();
        Console.Write("Enter Contact Info: ");
        string sContact = Console.ReadLine();

        var newSupplier = new marketmanagement.models.supplier { name = sName, contactinfo = sContact };
        db.suppliers.Add(newSupplier);
        db.SaveChanges();
        Console.WriteLine($"\nSucess Supplier '{sName}'registered with database ID: {newSupplier.id}\n");
    }
    else if (choice == "7")
    {
        Console.WriteLine("\nquick restock manager");

        Console.Write("Enter product barcode to restock ");
        string bc = Console.ReadLine();

        var dbProd = db.products.FirstOrDefault(p => p.barcode == bc);
        if (dbProd != null)
        {
            Console.Write($"Current stock is {dbProd.quantityinstock}. Enter amount to add ");
            if (int.TryParse(Console.ReadLine(), out int addedStock) && addedStock > 0)
            {
                dbProd.quantityinstock += addedStock;
                db.SaveChanges();

                trackingtree.UpdateProductInTree(bc, dbProd.title, dbProd.price, dbProd.quantityinstock);
                Console.WriteLine("\nDone\n");
            }
            else Console.WriteLine("Invalid amount enter");
        }
        else Console.WriteLine("Product barcode not found");
    }
    else if (choice == "8")
    {
        running = false;
        Console.WriteLine("shutting down market framework");
    }
    else
    {
        Console.WriteLine("invalid please try again");
    }
}