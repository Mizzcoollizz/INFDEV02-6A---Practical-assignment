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

        public void insertNewBuilding(Vector2 newBuilding) {
            if (this.getComparisonType().Equals("X")) {

                if (newBuilding.X < this.getBuildingX())
                {
                    if (this.getLeftChild() == null)
                    {
                        this.setLeftChild(new SpecialBuildingTreeNode("Y", newBuilding));
                    }
                    else {
                        this.getLeftChild().insertNewBuilding(newBuilding);
                    }
                }
                else {
                    if (this.getRightChild() == null)
                    {
                        this.setRightChild(new SpecialBuildingTreeNode("Y", newBuilding));
                    }
                    else {
                        this.getRightChild().insertNewBuilding(newBuilding);
                    }

                }
                
            } else if (this.getComparisonType().Equals("Y")){
                if (newBuilding.Y < this.getBuildingY())
                {
                    if (this.getLeftChild() == null)
                    {
                        this.setLeftChild(new SpecialBuildingTreeNode("X", newBuilding));
                    }
                    else {
                        this.getLeftChild().insertNewBuilding(newBuilding);
                    }
                }
                else {
                    if (this.getRightChild() == null)
                    {
                        this.setRightChild(new SpecialBuildingTreeNode("X", newBuilding));
                    }
                    else {
                        this.getRightChild().insertNewBuilding(newBuilding);
                    }
                }
                
            }
            
        }

        public void compare(List<Vector2> list, Vector2 house, float distance) {

            float xLeftRangeBorder = house.X - distance;
            float xRightRangeBorder = house.X + distance;

            float yLeftRangeBorder = house.Y - distance;
            float yRightRangeBorder = house.Y + distance;

            Boolean containsX = this.getBuildingX() >= xLeftRangeBorder && this.getBuildingX() <= xRightRangeBorder;
            Boolean containsY = this.getBuildingY() >= yLeftRangeBorder && this.getBuildingY() <= yRightRangeBorder;

            //If the building in the X and Y range, we should check if it is in the building range
            if (containsX && containsY) {
                //We do this using Vector.Distance
                if (Vector2.Distance(house, this.building) <= distance) {
                    list.Add(building);
                }
            }

            if (this.getComparisonType().Equals("X")) {
                if (containsX) {
                    if (this.getLeftChild() != null)
                    {
                        this.getLeftChild().compare(list, house, distance);
                    }
                    if (this.getRightChild() != null)
                    {
                        this.getRightChild().compare(list, house, distance);
                    }
                }

                if (getBuildingX() <= xLeftRangeBorder) {
                    if (this.getRightChild() != null)
                    {
                        this.getRightChild().compare(list, house, distance);
                    }
                }

                if (getBuildingX() >= xRightRangeBorder)
                {
                    if (this.getLeftChild() != null)
                    {
                        this.getLeftChild().compare(list, house, distance);
                    }
                }

            } else if (this.getComparisonType().Equals("Y")) {
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
                if (this.getBuildingY() <= yLeftRangeBorder) {
                    if (this.getRightChild() != null)
                    {
                        this.getRightChild().compare(list, house, distance);
                    }
                }

                if (this.getBuildingY() >= yRightRangeBorder) {
                    if (this.getLeftChild() != null)
                    {
                        this.getLeftChild().compare(list, house, distance);
                    }
                }
            }
            
        }

        public void checkTree(List<Vector2> list)
        {
            list.Add(this.building);
            if (this.getLeftChild() != null) {
                this.getLeftChild().checkTree(list);
            }
            if (this.getRightChild() != null)
            {
                this.getRightChild().checkTree(list);
            }
        }

    


    }
}
