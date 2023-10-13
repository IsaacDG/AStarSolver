/// <summary>
/// Created by Isaac Gonzalez
/// </summary>
using System;
using System.Drawing;
using System.Collections.Generic;
using System.Linq;
namespace AStarSolver
{
	class Program
	{
		public static void Main(string[] args)
		{
			solveMaze(args[0], args[1]);
		}
		
		/// <summary>
		/// This method is used to run the MazeRunner on the Maze.
		/// </summary>
		/// <param name="fromPath">The maze file.</param>
		/// <param name="toPath">Where to save the drawn solution.</param>
		public static void solveMaze(string fromPath, string toPath){
			try{
				Maze mz = new PixelMaze(@fromPath);
				MazeRunner solver = new MazeRunner();
				Console.WriteLine("MazeRunner is running the maze . . . Please wait!");
				Bitmap sol = solver.solvePixelMazeAStar((PixelMaze)mz);
				if(sol == null){
					Console.WriteLine("No Solution could be found!");
				} else {
					Console.WriteLine("Done, the solution is ready!");
					sol.Save(@toPath);	
				}
				Console.Write("\nPress any key to continue . . . ");
				Console.ReadKey(true);
			}catch(System.ArgumentException e){	//fromPath is bad.
				Console.WriteLine("Bad file path, please try again!");
				Console.Write("\nPress any key to continue . . . ");
				Console.ReadKey(true);
				return;
			}
			
		}
		
	}
}