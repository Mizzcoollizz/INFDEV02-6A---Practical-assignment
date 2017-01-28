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
        }

        //Assignment 1
        private static Vector2[] MergeSort(Vector2[] list, int startPoint, int endPoint, Vector2 house) {
         
            if (startPoint < endPoint) {
                int splitPoint = (startPoint + endPoint) / 2;
                MergeSort(list, startPoint, splitPoint, house);
                MergeSort(list, splitPoint + 1, endPoint, house);
                //Merge
                List<Vector2> listA = new List<Vector2>();
                List<Vector2> listB = new List<Vector2>();
                for (int i = startPoint; i <= splitPoint; i++) {
                    listA.Add(list[i]);
                }
                for (int i = splitPoint + 1; i <= endPoint; i++) {
                    listB.Add(list[i]);
                }
                
                for (int i = startPoint; i <= endPoint; i++) {
                    //Break if both lists are empty
                    if (!listA.Any() && !listB.Any())
                    {
                        break;
                    }
                    else if (listA.Any() && !listB.Any())
                    {
                        list[i] = listA.First();
                        listA.Remove(listA.First());
                    }
                    else if (!listA.Any() && listB.Any())
                    {
                        list[i] = listB.First();
                        listB.Remove(listB.First());
                    }
                    else {
                        float distanceA = Vector2.Distance(house, listA.First());
                        float distanceB = Vector2.Distance(house, listB.First());
                        if (distanceA < distanceB)
                        {
                            list[i] = listA.First();
                            listA.Remove(listA.First());
                        }
                        else {
                            list[i] = listB.First();
                            listB.Remove(listB.First());
                        }
                    }
                }
            }

            return list;
        }

        //Assignment 2
    private static IEnumerable<IEnumerable<Vector2>> FindSpecialBuildingsWithinDistanceFromHouse(
      IEnumerable<Vector2> specialBuildings, 
      IEnumerable<Tuple<Vector2, float>> housesAndDistances)
    {
            //Make the fist specialBuilding the root, with X as the type
            SpecialBuildingTreeNode root = new SpecialBuildingTreeNode("X", specialBuildings.First());
            //Create the tree with root a the root
            for (int i = 1; i < specialBuildings.Count(); i++)
            {
                Vector2 currentBuilding = specialBuildings.ElementAt(i);
                root.insertNewBuilding(currentBuilding);
            }

            List<List<Vector2>> returnList = new List<List<Vector2>>();

            //For every house, get the buildings in range
            for (int i = 0; i < housesAndDistances.Count(); i++)
            {
                Tuple<Vector2, float> houseAndDistance = housesAndDistances.ElementAt(i);
                List<Vector2> buildings = new List<Vector2>();
                root.compare(buildings, houseAndDistance.Item1, houseAndDistance.Item2);
                returnList.Insert(i, buildings);
              }
            return returnList;

        }

       
        //Assignment 3
     private static IEnumerable<Tuple<Vector2, Vector2>> FindRoute(Vector2 startingBuilding, 
      Vector2 destinationBuilding, IEnumerable<Tuple<Vector2, Vector2>> roads)
    {

            List<DijkstraNode> dijkstraNodes = new List<DijkstraNode>();
            //Add the start and end point to the list
            DijkstraNode startNode = new DijkstraNode(startingBuilding);
            dijkstraNodes.Add(startNode);
            DijkstraNode endNode = new DijkstraNode(destinationBuilding);
            
            dijkstraNodes.Add(endNode);
            //Add all the road start and end points to the list, as new DijkstraNodes:           
            foreach (Tuple<Vector2, Vector2> road in roads)
            {
                //To check that the location is not already in the list, we do the contains() method.
                //The equals() method is overridden in the DijkstraNode class
                DijkstraNode newNode = new DijkstraNode(road.Item1);
                DijkstraNode neighborNode = new DijkstraNode(road.Item2);
                if (!dijkstraNodes.Contains(newNode))
                {
                    dijkstraNodes.Add(newNode);
                }
                if (!dijkstraNodes.Contains(neighborNode)) {
                    dijkstraNodes.Add(neighborNode);
                }
                //Now we want to get the real DijkstraNodes from the list, and set the neighbors
                DijkstraNode foundNode = dijkstraNodes.Find(item => item.location.Equals(newNode.location));
                DijkstraNode foundNeighborNode = dijkstraNodes.Find(item => item.location.Equals(neighborNode.location));
                foundNode.addNeighbor(new Tuple<DijkstraNode, Tuple<Vector2, Vector2>>(foundNeighborNode, road));
                foundNeighborNode.addNeighbor(new Tuple<DijkstraNode, Tuple<Vector2, Vector2>>(foundNode, road));
            }

            //Define the startNode with the shortestPath that is empty, and the shortesPathLength that is 0
            startNode.shortestPath = new List<Tuple<Vector2, Vector2>>();
            startNode.shortestPathLength = 0;
            startNode.visitNode();
            return endNode.shortestPath;
            
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
