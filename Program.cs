using System;
using System.Collections.Generic;

namespace Acre_calculation
{

    class Program
    {
        public class Product
        {
            public string name { get; set; }
        }

        public class PlantedArea
        {
            public Product product { get; set; }

            public int ratio { get; set; }
        }

        public class FarmingSeason
        {
            public int id { get; set; }

            public List<Farm> farms { get; set; }

            public FarmingSeason()
            {
                farms = new List<Farm>();
            }
        }

        public class Farm
        {
            public int id { get; set; }

            public int area { get; set; }

            public List<PlantedArea> plantedAreas { get; set; }

            public Farm()
            {
                plantedAreas = new List<PlantedArea>();
            }

            public void addPlantedArea(Product product, int ratio)
            {
                plantedAreas.Add(new PlantedArea() { ratio = ratio, product = product });
            }
        }

        static void Main(string[] args)
        {
            int year = 4;

            int farmCount = 3;

            int farmArea = 10;

            List<Product> products = new List<Product>();

            products.Add(new Product() { name = "a" });
            products.Add(new Product() { name = "b" });
            products.Add(new Product() { name = "c" });

            List<FarmingSeason> farmingSeasons = new List<FarmingSeason>();

            for (int i = 1; i <= year; i++)
            {
                FarmingSeason farmingSeason = new FarmingSeason();
                farmingSeason.id = i;

                for (int j = 1; j <= farmCount; j++)
                {
                    Farm farm = new Farm();
                    farm.id = j;
                    farm.area = farmArea;

                    int maxRatio = 100;
                    int currentRatio = 0;

                    foreach (Product product in products)
                    {
                        bool wrongValue = false;
                        do
                        {
                            wrongValue = false;

                            Console.WriteLine("{2}. sene, {1}. tarlaya {0} ürününden % kaç ekim yapilacak? \nbu üründen ekim yapilmaycaksa bos birakin. kalan alan %{3}\n", product.name, j, i, maxRatio - currentRatio);

                            string inputStr = Console.ReadLine();

                            if (!string.IsNullOrEmpty(inputStr))
                            {
                                int ratio = 0;

                                if (int.TryParse(inputStr, out ratio))
                                {
                                    if (ratio <= 0)
                                    {
                                        wrongValue = true;
                                        Console.WriteLine("*** hatali giris ***\n");
                                        continue;
                                    }

                                    int calculatedValue = currentRatio + ratio;

                                    if (maxRatio < calculatedValue)
                                    {
                                        wrongValue = true;
                                        Console.WriteLine("*** tarlanin kalan ekilebilir alanindan daha büyük bir değer giremezsiniz. ***\n");
                                    }
                                    else
                                    {
                                        currentRatio = currentRatio + ratio;
                                        farm.addPlantedArea(product, ratio);
                                    }
                                }
                                else
                                {
                                    wrongValue = true;
                                    Console.WriteLine("*** girilen deger bir sayi degildir. ***\n");
                                }
                            }

                        } while (wrongValue);

                        if (currentRatio == maxRatio)
                            break;
                    }

                    farmingSeason.farms.Add(farm);
                }

                farmingSeasons.Add(farmingSeason);
            }


            Dictionary<string, double> totals = new Dictionary<string, double>();

            foreach (FarmingSeason season in farmingSeasons)
            {
                foreach (Farm farm in season.farms)
                {
                    foreach (PlantedArea area in farm.plantedAreas)
                    {
                        if (totals.ContainsKey(area.product.name))
                        {
                            totals[area.product.name] = totals[area.product.name] + (Convert.ToDouble(farm.area) * Convert.ToDouble(area.ratio) / 100);
                        }
                        else
                        {
                            totals.Add(area.product.name, Convert.ToDouble(farm.area) * Convert.ToDouble(area.ratio) / 100);
                        }
                    }
                }
            }

            foreach (var item in totals)
            {
                Console.WriteLine("{0} ürününden {1} dönüm", item.Key, item.Value);
            }

            Console.ReadLine();
        }
    }
}
