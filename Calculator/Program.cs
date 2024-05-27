using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Runtime.Remoting.Lifetime;
using System.Text;
using System.Threading.Tasks;


namespace Calculator
{
    internal class Program
    {
        static bool isInputCorrect(string aString, double a, string bString, double b, string operation, List<String> variables, List<String> varValues)
        {
            for (int i = 0; i < variables.Count; i++)
            {
                if (variables[i] == aString)
                {
                    aString = varValues[i];
                    break;
                } else if (variables[i] == bString)
                {
                    bString = varValues[i];
                }
            }

            if (!double.TryParse(aString, out a) || !double.TryParse(bString, out b))
            {
                return false;
            }
            else { 
                a = double.Parse(aString);
                b = double.Parse(bString);
                if (operation == "/" && b == 0.0d) //nelze delit 0
                    return false;
                else if (operation == "root" && a < 0)
                    return false;
                else if (operation == "log" && (a < 0 || b <= 0 || b == 1))
                    return false;

                return true;
            }
        }

        static void Main(string[] args)
        {
            Console.WriteLine("Program KALKULACKA.");
            Console.WriteLine("Vysvetlivky pro nektere operace: \n operace ^ je mocnina prvniho cisla o zakladu druhe cislo \n operace root je odmocnina z prvniho cisla o zakladu druhe cislo \n operace log je logaritmus z prvniho cisla o zakladu druhe cislo");
            bool continues = true;
            List<String> variables = new List<string>(); //pole pro vsechny uzivatelem zadane promenne
            List<String> varValues = new List<String>(); //pole pro vsechny uzivatelem zadane hodnoty promennych

            while (continues)
            {
                Console.WriteLine("Zadej novou promennou nebo zmen hodnotu stare. Pokud chces zmenit hodnotu jiz zadane promenne, napis jeji nazev. Pokud nechces zadavat promennou a chces rovnou prejit k pocitani, stiskni ENTER.");
                string newVariable =  Console.ReadLine();
                bool falseVariable = true; //overuje, zda je promenna zadana spravne (zda to neni cislo)
                int updateOldVariableIndex = -1; //overuje, zda nebyla stejna promenna zadana uz drive a pokud ano, je roven indexu teto promenne v poli promennych (v tom pripade bude pocitac pouze aktualizovat hodnotu teto promenne)

                while (newVariable != "" && falseVariable)
                {
                    falseVariable = false;
                    double newVariableDouble = 0.0d; //jen pro teoreticky pripad, kdyby uzivatel zadal jako nazev promenne cislo (to se delat nesmi) 
                    if(double.TryParse(newVariable, out newVariableDouble)) //zkontrolujeme, zda je (teoreticky) mozne prevest zadanou promennou na double - pokud ano, je to SPATNE
                    {
                        falseVariable = true;
                        Console.WriteLine("Cislo nemuze byt nazvem promenne. Pokud chces zadat novou promennou, zadej novy nazev. Pokud ne, stiskni ENTER.");
                        newVariable = Console.ReadLine();
                    } else
                    {
                        for (int i = 0; i < variables.Count; i++)
                        {
                            if (variables[i] == newVariable)
                            {
                                updateOldVariableIndex = i; 
                                break;
                            }
                        }
                    }


                    if (!falseVariable && updateOldVariableIndex == -1)
                        variables.Add(newVariable);
                }
                
                if(newVariable != "")
                {
                    Console.WriteLine("Zadej (novou) hodnotu teto promenne:");

                    bool falseVarValue = true; //overuje, zda je hodnota promenne zadana spravne (zda se jedna o cislo)
                    string newVarValue = "";
                    while (falseVarValue)
                    {
                        newVarValue = Console.ReadLine();
                        double newVarValueDouble = 0.0d;
                        if (double.TryParse(newVarValue, out newVarValueDouble)) //zkontrolujeme, zda je (teoreticky) mozne prevest zadanou hodnotu na double
                        {
                            falseVarValue = false;
                            if (updateOldVariableIndex != -1) //pokud promenna uz existujeme, nepridavame do pole promenmnych nic noveho, pouze aktualizujeme jeji hodnotu
                            {
                                varValues[updateOldVariableIndex] = newVarValue;
                                Console.WriteLine("Aktualizovana promenna: " + newVariable + " = " + newVarValue);
                            }
                            else
                            {
                                varValues.Add(newVarValue);
                                Console.WriteLine("Zapsana nova promenna: " + newVariable + " = " + newVarValue);
                            }

                        }
                        else
                        {
                            Console.WriteLine("Spatne zadana hodnota promenne, zadej hodnotu jeste jednou:");
                        }
                    }
                }
                


                Console.WriteLine("Zadej mat. operaci: \n(Moznosti operaci: +, -, *, /, ^, root, log)");
                bool falseOperation = true; //overuje, zda je mat. operace zadana spravne
                string[] operationOptions = { "+", "-", "*", "/", "^", "root", "log" }; //seznam povolenych mat. operaci
                string operation = Console.ReadLine(); ; //operace zadana uzivatelem

                while (falseOperation)
                {
                    

                    for (int i = 0; i < operationOptions.Length; i++)
                    {
                        if (operationOptions[i] == operation)
                        {
                            falseOperation = false;
                            break;
                        }
                    }

                    if (falseOperation)
                    {
                        Console.WriteLine("Nespravne zadana mat. operace, zadej ji jeste jednou (podle moznosti uvedenych nahore):");
                        operation = Console.ReadLine();

                    }
                }

                Console.WriteLine("Zadej prvni cislo/promennou:");
                string aString = Console.ReadLine();

                Console.WriteLine("Zadej druhe cislo/promennou:");
                string bString = Console.ReadLine();

                

                double a = 0.0, b = 0.0;

                while (!isInputCorrect(aString, a, bString, b, operation, variables, varValues) || falseOperation) //dokud cisla nejsou zadana spravne, chtej po nem vstup znovu
                {
                    Console.WriteLine("Nespravny vstup nebo reseni neexistuje, zadej cisla jeste jednou (operace zustava stejna):");
                    Console.WriteLine("Zadej prvni cislo:");
                    aString = Console.ReadLine();
                    Console.WriteLine("Zadej druhe cislo:");
                    bString = Console.ReadLine();
                }

                for (int i = 0; i < variables.Count; i++)
                {
                    if (variables[i] == aString)
                    {
                        aString = varValues[i];
                        break;
                    }
                    else if (variables[i] == bString)
                    {
                        bString = varValues[i];
                    }
                }
                a = double.Parse(aString);
                b = double.Parse(bString);
                double result = 0.0d;

                switch (operation)
                {
                    case "+":
                        result = a + b;
                        Console.WriteLine(result);
                        break;
                    case "-":
                        result = a - b;
                        Console.WriteLine(result);
                        break;
                    case "*":
                        result = a * b;
                        Console.WriteLine(result);
                        break;
                    case "/":
                        result = a / b;
                        Console.WriteLine(result);
                        break;
                    case "^":
                        result = Math.Pow(a, b);
                        Console.WriteLine(result);
                        break;
                    case "root":
                        result = Math.Pow(a, 1 / b);
                        Console.WriteLine(result);
                        break;
                    case "log":
                        result = Math.Log(a, b);
                        Console.WriteLine(result);
                        break;
                    default:
                        Console.WriteLine("Nespravne zadana mat. operace, zadej jeste jednou. Moznosti operaci: +, -, *, /, ^, root, log");
                        falseOperation = true;
                        break;
                }

                Console.WriteLine("Chcete pokracovat? Zadejte y pro pokracovani, cokoliv jineho program ukonci");
                string continueStr = Console.ReadLine();
                if (continueStr != "y")
                {
                    continues = false;
                }
            }
            Console.ReadKey(); //stisk klavesy pro skonceni 
        }
    }
}
