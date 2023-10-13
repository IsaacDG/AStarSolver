/// <summary>
/// Created by Isaac Gonzalez
/// </summary>
using System;
using System.Collections;

namespace AStarSolver
{
	/// <summary>
	/// This class is used to represent a node for use with the A* algorithm. It uses a Tuple of ints as a position
	/// so it can be used with a PixelMaze. It implements the comparable interface in order to specially
	/// compare nodes by their f value.
	/// </summary>
	public class Node : IComparable
	{
		/// <summary>
		/// To be used with A*, allows a path to be built from node to node.
		/// </summary>
		private Node parent;
		
		/// <summary>
		/// Tuple holding the x and y coordinates of a particular node.
		/// </summary>
		private Tuple<int, int> position;
		
		/// <summary>
		/// These values are to be used in the A* algorithm.
		/// f = g + h
		/// g = distance from this node to the beginning of the path
		/// h = distance from this node to the goal
		/// </summary>
		private float f, g, h;
		
		/// <summary>
		/// This constructor is used to construct a Node with a set position. f, g, and h are simply set to 0.
		/// </summary>
		/// <param name="position">The position to set.</param>
		public Node(Tuple<int, int> position){
			this.position = position;
			f = 0;
			g = 0;
			h = 0;
		}
		
		/// <summary>
		/// This method is used in order to compare Nodes by f value. This method was needed
		/// in order to correctly use the Min() with a HashSet of Nodes.
		/// </summary>
		/// <param name="obj">The Node to compare to</param>
		/// <returns>1 if this Node's f value is greater, -1 if it is smaller, and 0 if it is equal.</returns>
		int IComparable.CompareTo(object obj){
			Node n = (Node)obj;
			float otherF = n.getF();
			if(f > otherF)
				return 1;
			if(f < otherF)
				return -1;
			else
				return 0;
		}
		
		/// <summary>
		/// This method is used in order to be able to use a HashSet of Nodes. Instead of the HashCode being based
		/// on the object in memory, the hashcode is based off of the position Tuple, that way the A* algorithm can
		/// see if there are any nodes that share a position, but not necessarily an f value.
		/// </summary>
		/// <returns>The hash code based on the x and y coordinates of the Node.</returns>
		public override int GetHashCode(){
			unchecked{
				int hash = 17;
				hash = hash * 23 + position.Item1.GetHashCode();
				hash = hash * 23 + position.Item2.GetHashCode();
				return hash;
			}
		}
		
		/// <summary>
		/// This method is used to check if a Node is equal to another node. It only checks the coordinates of their
		/// positions. The f, g, and h values do not matter.
		/// </summary>
		/// <param name="obj">The node to compare to</param>
		/// <returns>True if the positions match, false otherwise.</returns>
		public override bool Equals(object obj){
			Node n = (Node)obj;
			Tuple<int,int> nPos = n.getPosition();
			bool left = position.Item1 == nPos.Item1;
			bool right = position.Item2 == nPos.Item2;
			return left && right;
			
			
		}
		
		/// <summary>
		/// Returns the position Tuple of the node.
		/// </summary>
		/// <returns>The tuple.</returns>
		public Tuple<int, int> getPosition(){
			return position;
		}
		
		/// <summary>
		/// Sets the Node's parent.
		/// </summary>
		/// <param name="parent">The Node to set parent to.</param>
		public void setParent(Node parent){
			this.parent = parent;
		}
		
		/// <summary>
		/// Returns the parent of the Node
		/// </summary>
		/// <returns>parent</returns>
		public Node getParent(){
			return parent;
		}
		
		/// <summary>
		/// Sets the value of g.
		/// </summary>
		/// <param name="g"></param>
		public void setG(float g){
			this.g = g;
		}
		
		/// <summary>
		/// Sets the value of f.
		/// </summary>
		/// <param name="f"></param>
		public void setF(float f){
			this.f = f;
		}
		
		/// <summary>
		/// Sets the value of h.
		/// </summary>
		/// <param name="h"></param>
		public void setH(float h){
			this.h = h;
		}
		
		/// <summary>
		/// Returns g
		/// </summary>
		/// <returns></returns>
		public float getG(){
			return g;
		}
		
		/// <summary>
		/// Returns f
		/// </summary>
		/// <returns></returns>
		public float getF(){
			return f;
		}
		
		/// <summary>
		/// Returns h.
		/// </summary>
		/// <returns></returns>
		public float getH(){
			return h;
		}
		
		
	}
}
