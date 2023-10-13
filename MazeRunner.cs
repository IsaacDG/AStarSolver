/// <summary>
/// Created by Isaac Gonzalez
/// </summary>
using System;
using System.Drawing;
using System.Collections.Generic;
using System.Linq;

namespace AStarSolver
{
	/// <summary>
	/// This class is intended to be used to solve Mazes. The algorithm it uses to solve the maze is the A* algorithm.
	/// </summary>
	public class MazeRunner
	{
		/// <summary>
		/// The solution of the maze. Dynamic to handle different representations of mazes. In 
		/// the case of PixelMaze, solution is a Bitmap.
		/// </summary>
		private dynamic solution;
		
		/// <summary>
		/// Method that uses the A* algorithm to solve the maze. 
		/// </summary>
		/// <param name="maze">The maze to be solved.</param>
		/// <returns>The bitmap containing the drawn path.</returns>
		public Bitmap solvePixelMazeAStar(PixelMaze maze){
			maze.findStartEnd();												//start and end needed for A*.
			Node sol = AStar(maze.getStart(), maze.getEnd(), (PixelMaze)maze);
			if(sol == null) return null;										//no solution found
			setSolution(traceNodesPixel(sol, maze.getMazeCopy()));
			return solution;
		}
		
		/// <summary>
		/// Method that traces the path found by A* by following the parent links starting from
		/// The last node in the path.
		/// </summary>
		/// <param name="end">The node to begin tracing from.</param>
		/// <param name="sol">The Bitmap to return</param>
		/// <returns>The Bitmap with the drawn path.</returns>
		public Bitmap traceNodesPixel(Node end, Bitmap sol){
			
			Node cursor = end;			
			
			//Using sequential traversal
			while(cursor != null){		
				Tuple<int, int> endPos = cursor.getPosition();
				sol.SetPixel(endPos.Item1, endPos.Item2, Color.Green);	//Coloring the pixel green.
				cursor = cursor.getParent();
				
			}
			return sol;
			
		}
		
		/// <summary>
		/// Returns the solution.
		/// </summary>
		/// <returns>solution</returns>
		public dynamic getSolution(){
			return solution;
		}
		
		/// <summary>
		/// Sets the solution. Private so it can not be tampered with.
		/// </summary>
		/// <param name="solution">The solution to set.</param>
		private void setSolution(dynamic solution){
			this.solution = solution;
		}
		
		/// <summary>
		/// This is the A* algorithm which is used to solve the maze. Successors are kept track of using a set called Open, while visited nodes
		/// are kept track of using a set called Closed. The algorithm loops while there are Nodes left in Open. Distance is calculated using the
		/// Manhattan method. 
		/// 
		/// Math used:
		/// g = distance from a node to the starting position of the search
		/// h = distance from a node to the goal.
		/// f = g + h
		/// </summary>
		/// <param name="start">Start position of the search</param>
		/// <param name="goalPos">The position we need to reach</param>
		/// <param name="maze">The maze we are dealing with</param>
		/// <returns>The last node in the path or null if no solution was found</returns>
		public dynamic AStar(Tuple<int, int> start, Tuple<int,int> goalPos, PixelMaze maze){
			if(start == null){
				Console.WriteLine("A start position could not be found! Terminating.");
				return null;
			}
			if(goalPos == null){
				Console.WriteLine("An end position could not be found! Terminating.");
				return null;
			}
			
			Dictionary<Tuple<int,int>, Node> open = new Dictionary<Tuple<int, int>, Node>(); //for instant access to Min position
			HashSet<Node> openPos = new HashSet<Node>();									 //used for access to Min() method in relation to f value of Nodes.
			HashSet<Tuple<int,int>> closed = new HashSet<Tuple<int,int>>();					 //contains nodes we have already processed successors for

			open.Add(start, new Node(start));
			openPos.Add(new Node(start));
			
			while(open.Count != 0){
				Tuple<int,int> currentPos = openPos.Min().getPosition();					//get node with min f
				Node current = open[currentPos];											//set the min node as current
				
				if(currentPos.Equals(goalPos)){
					return current;	//found end
				}
				
				openPos.Remove(current);
				open.Remove(currentPos);
				closed.Add(currentPos);
				Tuple<List<Tuple<int,int>>, HashSet<Tuple<int,int>>> surrPos = maze.getSurroundingEightEx(currentPos);	//gets surround positions with separate list for diagonals
				HashSet<Tuple<int,int>> diag = surrPos.Item2;						//HashSet of diagonal positions only.
				List<Tuple<int,int>> positions = surrPos.Item1;						//Coordinates of ALL surrounding positions
				List<Node> successors = new List<Node>();							//Creating list of Nodes with above coordinates for use in A*
				foreach(Tuple<int, int> pos in positions){
					successors.Add(new Node(pos));
				}
				
				bool[] valid = new bool[8]; 										//array containing statuses for {south, west, north, east, nw, ne, se, sw}
				validateSurrounding(valid, positions, maze);						//Fill out valid array accordingly

				for(int i = 0; i < successors.Count; i++){
					Tuple<int,int> successorPos = successors[i].getPosition();
					
					if(valid[i] && !closed.Contains(successorPos)){
						calculateF(diag, successors[i], current, goalPos);			//calculate f for successor
						
						if(!open.ContainsKey(successorPos)){						//if position is not already in open
							successors[i].setParent(current);
							open.Add(successorPos, successors[i]);
							openPos.Add(successors[i]);
						} else {
							Node inOpen = (Node)open[successorPos];					//if position is in open, compare this new g to the old g in open.
							
							if(inOpen.getG() > successors[i].getG()){				//if this g is smaller, set the current node as the parent and recalculate its f.
								successors[i].setParent(current);
								open[successorPos] = successors[i];
								openPos.Remove(successors[i]);						//replacing the old node f with new node f
								openPos.Add(successors[i]);			
							}
						}
					}
					
				}
				
			}
			return null;	//did not find a solution.
		}
		
		/// <summary>
		/// This method is used to make sure a position is a valid position. Diagonal positions are specially handled to make sure
		/// That they can be navigated to without cutting through.
		/// </summary>
		/// <param name="valid">The array to hold the results of validity</param>
		/// <param name="positions">The positions to check</param>
		/// <param name="maze">The maze to check positions in</param>
		public void validateSurrounding(bool[] valid, List<Tuple<int,int>> positions, Maze maze){
				
				for(int i = 0; i < positions.Count; i++){
					valid[i] = maze.validPosition(positions[i]);
				}
				
				if(valid[6] && !valid[0] && !valid[1]){	//check if sw is blocked
					valid[6] = false;
				}
				if(valid[4] && !valid[1] && !valid[2]){	//check if nw is blocked
					valid[4] = false;
				}
				if(valid[5] && !valid[2] && !valid[3]){	//check if ne is blocked
					valid[5] = false;
				}
				if(valid[7] && !valid[3] && !valid[0]){	//check if se is blocked
					valid[7] = false;
				}
		}
		
		/// <summary>
		/// Method used to calculate the f value for a successor node. The f value of a node is necessary in order
		/// to find the optimal path in A*.
		/// </summary>
		/// <param name="diag">A set containing the nodes that are diagonal, useful for calculating distance.</param>
		/// <param name="successor">The node to calculate f for.</param>
		/// <param name="current">Used for calculating g.</param>
		/// <param name="goalPos">Needed to calculate h.</param>
		public void calculateF(HashSet<Tuple<int,int>> diag, Node successor, Node current, Tuple<int,int> goalPos){
			float currG = current.getG();
			
			if(diag.Contains(successor.getPosition())){
				successor.setG(currG + 14);								//calculating distance from start, if diag set contains the position, add 14 instead of 10.
			} else {
				successor.setG(currG + 10);			
			}
			successor.setH(distFromGoal(successor.getPosition(), goalPos));			//calculate dist from goal
			successor.setF(currG + successor.getH());					//f = dist from start + dist from goal
		}
		
		/// <summary>
		/// Method used to calculate a coordinate's distance from the coordinate of the goal.
		/// </summary>
		/// <param name="pos">The position we are at</param>
		/// <param name="goal">The goal position</param>
		/// <returns>The absolute value of the total number of pixels the position is from the goal position.</returns>
		public int distFromGoal(Tuple<int,int> pos, Tuple<int, int> goal){
			
			return Math.Abs((pos.Item1 - goal.Item1) + (pos.Item2 - goal.Item2)) * 10;	//calculates total number of blocks, horizontal and vertical
			
		}
	}
}
