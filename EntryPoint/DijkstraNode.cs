using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace EntryPoint
{
    class DijkstraNode
    {
        public List<Tuple<Vector2, Vector2>> shortestPath { get; set;} = null;
        public float shortestPathLength { get; set; } = float.PositiveInfinity;
        private List<Tuple<DijkstraNode, Tuple<Vector2, Vector2>>> neighbors = new List<Tuple<DijkstraNode, Tuple<Vector2, Vector2>>>();
        public Vector2 location { get; set; } = Vector2.Zero;
        public Boolean visited { get; set; } = false;

        public void addNeighbor(Tuple<DijkstraNode, Tuple<Vector2,Vector2>> neighbor){
            if (!neighbors.Contains(neighbor)) {
                neighbors.Add(neighbor);
            }
          }

        public DijkstraNode(Vector2 location) {
            this.location = location;
            
        }

        //We have overridden the Equals() method, because of the Contains() method in the list
        public override bool Equals(object obj)
        {
            if (obj is DijkstraNode)
            {
                DijkstraNode otherNode = (DijkstraNode)obj;
                return otherNode.location.Equals(this.location);
            }
            else {
                return false;
            }            
        }

        public void visitNode() {

            this.visited = true;

            foreach (Tuple<DijkstraNode, Tuple<Vector2, Vector2>> neighbor in neighbors){
                //If neccecary, update the shortest path
                float shortestPathToNeighbor = this.shortestPathLength + 1;
                if (shortestPathToNeighbor < neighbor.Item1.shortestPathLength)
                {
                    List<Tuple<Vector2, Vector2>> newShortestPathList = new List<Tuple<Vector2, Vector2>>(this.shortestPath);
                    //Add the road to the neighbor to the new shortestPath
                    newShortestPathList.Add(neighbor.Item2);
                    neighbor.Item1.shortestPath = newShortestPathList;
                    neighbor.Item1.shortestPathLength = newShortestPathList.Count();
                }                   
            }
            
            List<Tuple<DijkstraNode, Tuple<Vector2, Vector2>>> neighborsToVisitList = new List<Tuple<DijkstraNode, Tuple<Vector2, Vector2>>>(neighbors);
            while (neighborsToVisitList.Any()) {
                Tuple<DijkstraNode, Tuple<Vector2, Vector2>> neighborToVisitNext = getNearestNeighbor(neighborsToVisitList);
                if (!neighborToVisitNext.Item1.visited) {
                    neighborToVisitNext.Item1.visitNode();
                }
                neighborsToVisitList.Remove(neighborToVisitNext);
            }

            
        }

        //Method for selecting the nearest neighbor.
        private Tuple<DijkstraNode, Tuple<Vector2, Vector2>> getNearestNeighbor(List<Tuple<DijkstraNode, Tuple<Vector2, Vector2>>> list)
        {
            float lowestDistance = float.PositiveInfinity;
            Tuple<DijkstraNode, Tuple<Vector2, Vector2>> currentClosestNeighbor = null;
            foreach (Tuple<DijkstraNode, Tuple<Vector2, Vector2>> neighbor in list) {
                if (Vector2.Distance(neighbor.Item2.Item1, neighbor.Item2.Item2) < lowestDistance) {
                    currentClosestNeighbor = neighbor;
                }
            }

            return currentClosestNeighbor;

        }
    }


}
