using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using Copy_cat.Model;
namespace Copy_cat
{
    class Program
    {
        static void Main(string[] args)
        {
            Record temp = null;
            List<Record> records = new List<Record>();
            //initializing chrome instance.
            wait:
            Console.WriteLine("System going to wait");
            while(true)
            {
                String wait = DateTime.Now.Hour.ToString() + DateTime.Now.Minute.ToString();
                if (wait == "200")
                {
                    break;
                }
                
            }
            Console.Clear();
            Console.WriteLine("Wait ended, system starting processing..!");
            IWebDriver gc=null;
            try
            {
                 gc = new ChromeDriver();
            }
            catch { }
            //loggin into client space
            try
            {
                //...loging in 
                gc.Navigate().GoToUrl("https://cwp.clientspace.net/Next/Login");
                gc.FindElement(By.Name("LoginID")).SendKeys("lightbot");
                gc.FindElement(By.Name("Password")).SendKeys("RPAuser!");
                gc.FindElement(By.Name("Password")).SendKeys(Keys.Enter);
                //.............
            }
            catch (Exception ex)
            {
                Console.WriteLine("Login into clientspace failed.");
            }
            // Opening up Organization tab
            try
            {
                gc.Navigate().GoToUrl("https://cwp.clientspace.net/Next/Organizations");
                System.Threading.Thread.Sleep(5000);
            }
            catch
            {
                Console.WriteLine("Opening organization tab failed.");
            }
            //adjusting search parameters
            try
            {
                gc.FindElement(By.XPath("//*[@id='dropdownMenu1']")).Click();
                gc.FindElement(By.XPath("//*[@id='Category']")).SendKeys("Duplicate");
               // System.Threading.Thread.Sleep(2000);
                gc.FindElement(By.XPath("//*[@id='Category']")).SendKeys(Keys.Enter);
                System.Threading.Thread.Sleep(3000);
            }
            catch
            {
                Console.WriteLine("Search parameter adjustment failed");
            }
            
            //extracting data
            try
            {
                int i = 0;
                while (true)
                {
                    foreach (IWebElement e in gc.FindElement(By.XPath("//*[@id='orgSearchList']/div[2]/table")).FindElements(By.TagName("tr")))
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.Write(i);
                        ICollection<IWebElement> cols = e.FindElements(By.TagName("td"));
                        temp = new Record(cols.ElementAt(1).Text, cols.ElementAt(3).Text);
                        temp.print();
                        records.Add(temp);
                        temp = null;
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.WriteLine("-------------");
                        i++;
                    }
                    string number = gc.FindElement(By.XPath("//*[@id='orgSearchList']/div[3]/span")).Text;
                    number = number.Replace("of ", "").Replace("- ", "").Replace("items", "");
                    string[] parts = number.Split(' ');
                    Console.WriteLine(parts[1] + parts[2]);
                    System.Threading.Thread.Sleep(2000);
                    if(parts[1]==parts[2])
                    {
                        Console.WriteLine("End of data");
                        System.Threading.Thread.Sleep(4000);
                        break;
                    }
                    if (gc.FindElement(By.XPath("//*[@id='orgSearchList']/div[3]/a[3]")).Enabled == true)
                    {
                        gc.FindElement(By.XPath("//*[@id='orgSearchList']/div[3]/a[3]")).Click();
                        System.Threading.Thread.Sleep(7000);
                    }
                    else
                    {
                        break;
                    }
                }
            }
            catch
            {
                Console.WriteLine("Data extraction failed");
            }
            
            Console.Clear();
            Console.WriteLine("Printing Filtered data");
            foreach( Record r in records.Where(s=>s.status=="Lead" || s.status=="DM Pipeline"))
            {
                r.print();
                try
                {
                    gc.Navigate().GoToUrl("https://cwp.clientspace.net/Next/Organizations");
                    System.Threading.Thread.Sleep(5000);
                    gc.FindElement(By.XPath("//*[@id='dropdownMenu1']")).Click();
                    gc.FindElement(By.XPath("//*[@id='Organization']")).SendKeys(r.CompanyName);
                    System.Threading.Thread.Sleep(500);
                    gc.FindElement(By.XPath("//*[@id='Category']")).SendKeys("Duplicate");
                    System.Threading.Thread.Sleep(2000);
                    gc.FindElement(By.XPath("//*[@id='Category']")).SendKeys(Keys.Enter);
                    System.Threading.Thread.Sleep(2000);
                    gc.FindElement(By.XPath("//*[@id='orgSearchList']/div[2]/table/tbody/tr/td[1]/div/a/span")).Click();
                    System.Threading.Thread.Sleep(4000);
                    gc.FindElement(By.XPath("//*[@id='tabGeneral']/div[2]/div[1]/div[2]/div/span")).Click();
                    System.Threading.Thread.Sleep(5000);
                }
                catch
                {

                }

            }
            Console.WriteLine("Process completed");
            try
            {
                gc.Close();
                gc.Dispose();

            }
            catch
            {
                Console.WriteLine("Chrome closing failed");
            }
            System.Threading.Thread.Sleep(4000);
            Console.Clear();
            goto wait;
        }
    }
}
