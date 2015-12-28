using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EntryPoint
{
#if WINDOWS || LINUX
  public static class Program
  {
    [STAThread]
    static void Main()
    {
      var fullscreen = false;
      read_input:
      switch (Microsoft.VisualBasic.Interaction.InputBox("Which assignment shall run next? (1, 2, 3, 4, or q for quit)", "Choose assignment", VirtualCity.GetInitialValue()))
      {
        case "1":
          using (var game = VirtualCity.RunAssignment1(SortSpecialBuildingsByDistance, fullscreen))
            game.Run();
          break;
        case "2":
          using (var game = VirtualCity.RunAssignment2(FindSpecialBuildingsWithinDistanceFromHouse, fullscreen))
            game.Run();
          break;
        case "3":
          using (var game = VirtualCity.RunAssignment3(FindRoute, fullscreen))
            game.Run();
          break;
        case "4":
          using (var game = VirtualCity.RunAssignment4(FindRoutesToAll, fullscreen))
            game.Run();
          break;
        case "q":
          return;
      }
      goto read_input;
    }

    private static IEnumerable<Vector2> SortSpecialBuildingsByDistance(Vector2 house, IEnumerable<Vector2> specialBuildings)
    {

            return MergeSort(specialBuildings.ToArray(), 0, specialBuildings.Count() - 1, house);

            //List<List<Vector2>> currentList = new List<List<Vector2>>();
            ////First add all the (split) items to the current list.
            //foreach (Vector2 item in specialBuildings)
            //{
            //    List<Vector2> listWithItem = (new List<Vector2>());
            //    listWithItem.Add(item);
            //    currentList.Add(listWithItem);
            //}

            ////Then merge the lists to one big list 
            //while (currentList.Count() > 1)
            //{
            //    List<List<Vector2>> newList = new List<List<Vector2>>();
            //    int pairsToMake = 0;
            //    if (currentList.Count % 2 == 0)
            //    {
            //        pairsToMake = currentList.Count();
            //    }
            //    else {
            //        pairsToMake = currentList.Count() - 1;
            //        newList.Add(currentList.Last());
            //    }

            //    for (int listToPair = 0; listToPair < pairsToMake; listToPair += 2)
            //    {
            //        List<Vector2> newPair = new List<Vector2>();
            //        List<Vector2> listA = currentList.ElementAt(listToPair);
            //        List<Vector2> listB = currentList.ElementAt(listToPair + 1);
            //        while (listA.Any() || listB.Any())
            //        {
            //            if (!listA.Any())
            //            {
            //                newPair.AddRange(listB);
            //                listB.Clear();
            //            }
            //            else if (!listB.Any())
            //            {
            //                newPair.AddRange(listA);
            //                listA.Clear();
            //            }
            //            else {
            //                Vector2 itemA = listA.First();
            //                Vector2 itemB = listB.First();
            //                double distanceItemA = Math.Sqrt(
            //                        Math.Pow((house.X - itemA.X), 2)
            //                        +
            //                        Math.Pow((house.Y - itemA.Y), 2));
            //                double distanceItemB = Math.Sqrt(
            //                        Math.Pow((house.X - itemB.X), 2)
            //                        +
            //                        Math.Pow((house.Y - itemB.Y), 2));
            //                if (distanceItemA < distanceItemB)
            //                {
            //                    newPair.Add(itemA);
            //                    listA.Remove(itemA);

            //                }
            //                else {
            //                    newPair.Add(itemB);
            //                    listB.Remove(itemB);
            //                }

            //            }

            //        }
            //        newList.Add(newPair);
            //    }
            //    currentList = newList;

            //}

            //return currentList.First();

        }

        private static Vector2[] MergeSort(Vector2[] list, int startPoint, int endPoint, Vector2 house) {

         
            if (startPoint < endPoint) {
                int splitPoint = (startPoint + endPoint) / 2;
                MergeSort(list, startPoint, splitPoint, house);
                MergeSort(list, splitPoint + 1, endPoint, house);
                //Merge:
                //Create the lists
                List<Vector2> listA = new List<Vector2>();
                for (int i = startPoint; i <= splitPoint; i++) {
                    listA.Add(list.ElementAt(i));
                }
                List<Vector2> listB = new List<Vector2>();
                for (int i = splitPoint + 1; i <= endPoint; i++) {
                    listB.Add(list.ElementAt(i));
                }
                //Sort the lists
                
                
                for(int i = startPoint; i <= endPoint; i++)
                {
                    
                    if (!listA.Any() && !listB.Any())
                    {
                        break;
                    }
                    else if (!listA.Any() && listB.Any())
                    {
                        Vector2 item = listB.First();
                        list[i] = item;
                        listB.Remove(item);
                    }
                    else if (listA.Any() && !listB.Any())
                    {
                        Vector2 item = listA.First();
                        list[i] = item;
                        listA.Remove(item);
                    }
                    else {

                        Vector2 itemA = listA.First();
                        Vector2 itemB = listB.First();
                        double distanceItemA = Math.Sqrt(
                                Math.Pow((house.X - itemA.X), 2)
                                +
                                Math.Pow((house.Y - itemA.Y), 2));
                        double distanceItemB = Math.Sqrt(
                                Math.Pow((house.X - itemB.X), 2)
                                +
                                Math.Pow((house.Y - itemB.Y), 2));
                        if (distanceItemA < distanceItemB)
                        {
                            list[i] = itemA;
                            listA.Remove(itemA);
                        }
                        else {
                            list[i] = itemB;
                            listB.Remove(itemB);
                        }
                    }

                }
                
            }

            return list;
        }

    private static IEnumerable<IEnumerable<Vector2>> FindSpecialBuildingsWithinDistanceFromHouse(
      IEnumerable<Vector2> specialBuildings, 
      IEnumerable<Tuple<Vector2, float>> housesAndDistances)
    {
      return
          from h in housesAndDistances
          select
            from s in specialBuildings
            where Vector2.Distance(h.Item1, s) <= h.Item2
            select s;
    }

    private static IEnumerable<Tuple<Vector2, Vector2>> FindRoute(Vector2 startingBuilding, 
      Vector2 destinationBuilding, IEnumerable<Tuple<Vector2, Vector2>> roads)
    {
      var startingRoad = roads.Where(x => x.Item1.Equals(startingBuilding)).First();
      List<Tuple<Vector2, Vector2>> fakeBestPath = new List<Tuple<Vector2, Vector2>>() { startingRoad };
      var prevRoad = startingRoad;
      for (int i = 0; i < 30; i++)
      {
        prevRoad = (roads.Where(x => x.Item1.Equals(prevRoad.Item2)).OrderBy(x => Vector2.Distance(x.Item2, destinationBuilding)).First());
        fakeBestPath.Add(prevRoad);
      }
      return fakeBestPath;
    }

    private static IEnumerable<IEnumerable<Tuple<Vector2, Vector2>>> FindRoutesToAll(Vector2 startingBuilding, 
      IEnumerable<Vector2> destinationBuildings, IEnumerable<Tuple<Vector2, Vector2>> roads)
    {
      List<List<Tuple<Vector2, Vector2>>> result = new List<List<Tuple<Vector2, Vector2>>>();
      foreach (var d in destinationBuildings)
      {
        var startingRoad = roads.Where(x => x.Item1.Equals(startingBuilding)).First();
        List<Tuple<Vector2, Vector2>> fakeBestPath = new List<Tuple<Vector2, Vector2>>() { startingRoad };
        var prevRoad = startingRoad;
        for (int i = 0; i < 30; i++)
        {
          prevRoad = (roads.Where(x => x.Item1.Equals(prevRoad.Item2)).OrderBy(x => Vector2.Distance(x.Item2, d)).First());
          fakeBestPath.Add(prevRoad);
        }
        result.Add(fakeBestPath);
      }
      return result;
    }
  }
#endif
}
