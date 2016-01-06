using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace EntryPoint
{
    class SpecialBuildingTreeNode
    {
        SpecialBuildingTreeNode leftChild = null;
        SpecialBuildingTreeNode rightChild = null;
        String type;
        Vector2 building;

        public String getComparisonType()
        {
            return this.type;
        }

        public SpecialBuildingTreeNode(String type, Vector2 building)
        {
            this.type = type;
            this.building = building;
        }

        public void setLeftChild(SpecialBuildingTreeNode left)
        {
            this.leftChild = left;
        }

        public void setRightChild(SpecialBuildingTreeNode right)
        {
            this.rightChild = right;
        }

        public SpecialBuildingTreeNode getLeftChild()
        {
            return this.leftChild;
        }

        public SpecialBuildingTreeNode getRightChild()
        {
            return this.rightChild;
        }

        public float getBuildingX()
        {
            return this.building.X;
        }

        public float getBuildingY() {
            return this.building.Y;
        }

        public void insertNewBuilding(Vector2 newBuilding)
        {
            

            if (this.getComparisonType().Equals("X"))
            {
                if (newBuilding.X <= this.getBuildingX())
                {
                    if (this.getLeftChild() != null)
                    {
                        this.getLeftChild().insertNewBuilding(newBuilding);
                    }
                    else {
                        this.setLeftChild(new SpecialBuildingTreeNode("Y", newBuilding));

                    }
                }
                else {
                    if (this.getRightChild() != null)
                    {
                        this.getRightChild().insertNewBuilding(newBuilding);
                    }
                    else {
                        this.setRightChild(new SpecialBuildingTreeNode("Y", newBuilding));
                    }
                }
            }
            else if (this.getComparisonType().Equals("Y")) {
                if (newBuilding.Y <= this.getBuildingY())
                {
                    if (this.getLeftChild() != null)
                    {
                        this.getLeftChild().insertNewBuilding(newBuilding);
                    }
                    else {
                        this.setLeftChild(new SpecialBuildingTreeNode("X", newBuilding));
                    }
                }
                else {
                    if (this.getRightChild() != null)
                    {
                        this.getRightChild().insertNewBuilding(newBuilding);
                    }
                    else {
                        this.setRightChild(new SpecialBuildingTreeNode("X", newBuilding));
                    }
                }
            }
        }

        public void compare(List<Vector2> list, Vector2 house, float distance) {

            IEnumerable<int> xRange = Enumerable.Range((int) house.X - (int) distance, (int) house.X + (int) distance);
            IEnumerable<int> yRange = Enumerable.Range((int) house.Y - (int) distance, (int) house.Y + (int) distance);

            Boolean containsX = xRange.Contains((int) this.getBuildingX());
            Boolean containsY = yRange.Contains((int) this.getBuildingY());

            if (containsX && containsY) {
                //Do extra check using Vector.Distance()
                if (Vector2.Distance(house, this.building) <= distance) {
                    list.Add(this.building);
                }
            }

            if (this.getComparisonType().Equals("X"))
            {
                if (containsX)
                {
                    if (this.getLeftChild() != null)
                    {
                        this.getLeftChild().compare(list, house, distance);
                    }
                    if (this.getRightChild() != null)
                    {
                        this.getRightChild().compare(list, house, distance);
                    }
                }
                else {
                    if (house.X <= this.getBuildingX())
                    {
                        if (this.getLeftChild() != null)
                        {
                            this.getLeftChild().compare(list, house, distance);
                        }
                    }
                    else if (house.X > this.getBuildingX())
                    {
                        if(this.getRightChild() != null)
                        {
                            this.getRightChild().compare(list, house, distance);
                        }
                    }
                } 
            }
            else if (this.getComparisonType().Equals("Y"))
            {
                if (containsY)
                {
                    if (this.getLeftChild() != null)
                    {
                        this.getLeftChild().compare(list, house, distance);
                    }
                    if (this.getRightChild() != null)
                    {
                        this.getRightChild().compare(list, house, distance);
                    }
                }
                else {
                    if (house.Y <= this.getBuildingY())
                    {
                        if (this.getLeftChild() != null)
                        {
                            this.getLeftChild().compare(list, house, distance);
                        }
                    }
                    else if (house.Y > this.getBuildingY())
                    {
                        if (this.getRightChild() != null)
                        {
                            this.getRightChild().compare(list, house, distance);
                        }
                    }
                }
            }
        }        
    }
}
