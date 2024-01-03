using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbstractFactoryConsole
{
    class Program
    {
        abstract class Burguer { }
        abstract class Dessert { }
        abstract class RecipeFactory {
            public abstract Burguer CreateBurguer();
            public abstract Dessert CreateDessert();
            
        }
        class SteaakBurbuer : Burguer { }
        class CreamBluer: Dessert { }
        class AdultCuisineFactory : RecipeFactory
        {
            public override Burguer CreateBurguer()
            {
                return new SteaakBurbuer();
            }

            public override Dessert CreateDessert()
            {
                return new CreamBluer();
            }
        }
        class KidBurguer : Burguer { }
        class IceCream : Dessert { }
        class KidCusineFactory : RecipeFactory
        {
            public override Burguer CreateBurguer()
            {
                return new KidBurguer();
            }

            public override Dessert CreateDessert()
            {
                return new IceCream();
            }
        }

        static void Main(string[] args)
        {
            Console.WriteLine("Who are you?");
            Console.WriteLine("A - Adult");
            Console.WriteLine("K - Kid");
            char result = Console.ReadKey().KeyChar;
            RecipeFactory factory = null;
            
            switch (result)
            {
                case 'A':
                    factory = new AdultCuisineFactory();
                    break;
                case 'K':
                    factory = new KidCusineFactory();
                    break;
                default:
                    break;
            }


            var burguer = factory.CreateBurguer();
            var dessert = factory.CreateDessert();

            Console.WriteLine("");
            Console.WriteLine("Burguer: " + burguer.GetType().Name);
            Console.WriteLine("Dessert: " + dessert.GetType().Name);
            Console.ReadKey();
        }
    }
}
