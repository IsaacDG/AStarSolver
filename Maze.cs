/// <summary>
/// Created by Isaac Gonzalez
/// </summary>
using System;
using System.Drawing;
using System.Collections.Generic;

namespace AStarSolver
{
	
	/// <summary>
	/// This is an abstract class representing a Maze. Makes use of the dynamic keyword
	/// in order to allow for subclasses to represent mazes in ways other than pixels.
	/// </summary>
	public abstract class Maze
	{
		/// <summary>
		/// The mazeData member variable is the representation of the maze itself. 
		/// In this program, mazeData is a Bitmap object.
		/// Protected so subclasses of Maze can have direct access to the maze data.
		/// </summary>
		protected dynamic mazeData;
		
		/// <summary>
		/// String that holds the filePath of the mazeData.
		/// </summary>
		private String filePath;
		
		/// <summary>
		/// Member variable holding the 'start' of the maze. In the case of PixelMaze,
		/// it will be a Tuple of ints holding the coordinates of a Red pixel.
		/// </summary>
		protected dynamic startPos;
		
		/// <summary>
		/// Member variable holding the 'end' of the maze. This is for use with the A* algorithm.
		/// </summary>
		protected dynamic endPos;
		
		/// <summary>
		/// Constructor that takes in a String object and sets it to the filePath member variable.
		/// </summary>
		/// <param name="path"></param>
		public Maze(String path)
		{
			filePath = path;
		}
		
		/// <summary>
		/// Returns the startPos member variable.
		/// </summary>
		/// <returns>startPos</returns>
		public dynamic getStart(){
			return startPos;
		}
		
		/// <summary>
		/// Returns the endPos member variable.
		/// </summary>
		/// <returns>endPos</returns>
		public dynamic getEnd(){
			return endPos;
		}
		
				/// <summary>
		/// Abstract signature intended to be overridden. In the case of PixelMaze,
		/// the surrounding positions are the coordinates in the four cardinal directions
		/// around the current coordinate.
		/// </summary>
		/// <param name="pos">The current position</param>
		/// <returns>Depends on subclass implementation.</returns>
		public abstract dynamic getSurroundingPositions(dynamic pos);
		
		/// <summary>
		/// Intended to be overridden. Should return a copy of the mazeData.
		/// </summary>
		/// <returns>Copy of mazeData</returns>
		public abstract dynamic getMazeCopy();
		
		/// <summary>
		/// Method to find the start of a maze.
		/// </summary>
		public abstract void findStart();
		
		/// <summary>
		/// Method to check if a particular position is at the end of the maze.
		/// </summary>
		/// <param name="position"></param>
		/// <returns></returns>
		public abstract bool isEnd(dynamic position);
		
		/// <summary>
		/// Method to check if a particular position is a valid position. Meaning the position
		/// is not a wall or some other type of blockage.
		/// </summary>
		/// <param name="position"></param>
		/// <returns></returns>
		public abstract bool validPosition(dynamic position);
		
				
	}
	
}
