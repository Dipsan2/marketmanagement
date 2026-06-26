using System;
using System.Linq;

namespace marketmanagement.datastructures
{
    public class producttree
    {
        private productnode rootnode;

        public producttree()
        {
            rootnode = null;
        }

        public void insertproduct(marketmanagement.models.product newproduct)
        {
            rootnode = insertrecursive(rootnode, newproduct);
        }

        private productnode insertrecursive(productnode current, marketmanagement.models.product newproduct)
        {
            if (current == null)
            {
                return new productnode(newproduct);
            }

            int comparison = string.Compare(newproduct.barcode, current.productdata.barcode);

            if (comparison < 0)
            {
                current.leftchild = insertrecursive(current.leftchild, newproduct);
            }
            else if (comparison > 0)
            {
                current.rightchild = insertrecursive(current.rightchild, newproduct);
            }

            return current;
        }

        public marketmanagement.models.product searchbybarcode(string barcode)
        {
            return searchrecursive(rootnode, barcode);
        }

        private marketmanagement.models.product searchrecursive(productnode current, string barcode)
        {
            if (current == null) return null;

            int comparison = string.Compare(barcode, current.productdata.barcode);

            if (comparison == 0)
            {
                return current.productdata;
            }

            if (comparison < 0)
            {
                return searchrecursive(current.leftchild, barcode);
            }
            else
            {
                return searchrecursive(current.rightchild, barcode);
            }
        }

        public void searchbynamepartial(string searchterm)
        {
            Console.WriteLine($"\ntree traversal search results for'{searchterm}'");
            int matchcount = 0;
            traversandmatch(rootnode, searchterm.ToLower(), ref matchcount);

            if (matchcount == 0)
            {
                Console.WriteLine("no products matched your search description");
            }
            Console.WriteLine();
        }

        private void traversandmatch(productnode current, string term, ref int count)
        {
            if (current == null) return;

            traversandmatch(current.leftchild, term, ref count);

            if (current.productdata.title.ToLower().Contains(term))
            {
                Console.WriteLine($"match found Barcode{current.productdata.barcode} | Name: {current.productdata.title} | Price: £{current.productdata.price} | Stock: {current.productdata.quantityinstock}");
                count++;
            }

            traversandmatch(current.rightchild, term, ref count);
        }

        public bool UpdateProductInTree(string barcode, string newTitle, decimal newPrice, int newStock)
        {
            var foundProduct = searchbybarcode(barcode);
            if (foundProduct != null)
            {
                foundProduct.title = newTitle;
                foundProduct.price = newPrice;
                foundProduct.quantityinstock = newStock;
                return true;
            }
            return false;
        }

        public void DeleteProduct(string barcode)
        {
            rootnode = DeleteRecursive(rootnode, barcode);
        }

        private productnode DeleteRecursive(productnode root, string barcode)
        {
            if (root == null) return root;

            int comparison = string.Compare(barcode, root.productdata.barcode);

            if (comparison < 0)
            {
                root.leftchild = DeleteRecursive(root.leftchild, barcode);
            }
            else if (comparison > 0)
            {
                root.rightchild = DeleteRecursive(root.rightchild, barcode);
            }
            else
            {
                if (root.leftchild == null) return root.rightchild;
                if (root.rightchild == null) return root.leftchild;

                root.productdata = MinValue(root.rightchild);
                root.rightchild = DeleteRecursive(root.rightchild, root.productdata.barcode);
            }
            return root;
        }

        private marketmanagement.models.product MinValue(productnode root)
        {
            var minv = root.productdata;
            while (root.leftchild != null)
            {
                minv = root.leftchild.productdata;
                root = root.leftchild;
            }
            return minv;
        }

       
        public void generatemanagementreports(int stockthreshold)
        {
            Console.WriteLine($"\nlow stock alert log (threshold: < {stockthreshold} units)");
            int lowStockCount = 0;
            checklowstock(rootnode, stockthreshold, ref lowStockCount);
            if (lowStockCount == 0) Console.WriteLine("all quantities are good.");

            Console.WriteLine("\n expiration & restock schedule");
            int scheduleCount = 0;
            printschedule(rootnode, ref scheduleCount);
            if (scheduleCount == 0) Console.WriteLine("no dated items in memory cache");
            Console.WriteLine();
        }

        private void checklowstock(productnode current, int threshold, ref int count)
        {
            if (current == null) return;

            checklowstock(current.leftchild, threshold, ref count);

            if (current.productdata.quantityinstock < threshold)
            {
                Console.WriteLine($"ALERT: {current.productdata.title} (Barcode: {current.productdata.barcode}) is running low! Only {current.productdata.quantityinstock} units left.");
                count++;
            }

            checklowstock(current.rightchild, threshold, ref count);
        }

        private void printschedule(productnode current, ref int count)
        {
            if (current == null) return;

            printschedule(current.leftchild, ref count);

            string dateString = current.productdata.expiryorrestockdate.ToString("yyyy-MM-dd");
            Console.WriteLine($"Product: {current.productdata.title} | Barcode: {current.productdata.barcode} | Scheduled Date: {dateString}");
            count++;

            printschedule(current.rightchild, ref count);
        }
    }
}