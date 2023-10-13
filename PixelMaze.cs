/// <summary>
/// Created by Isaac Gonzalez
/// </summary>
using System;
using System.Collections.Generic;
using System.Drawing;

namespace AStarSolver
{
	public enum PixelColors {Black, White, Blue, Red};	//enum to represent the colors of the Bitmap
	/// <summary>
	///	This class is used to represent a maze with a Bitmap. It is a subclass of the abstract Maze class in order to be
	/// used with the MazeRunner class.
	/// </summary>
	public class PixelMaze : Maze
	{
		
		/// <summary>
		/// This constructor simply passes the String used as the filepath to the super constructor.
		/// </summary>
		/// <param name="path">The filepath of the mazedata.</param>
		public PixelMaze(String path) : base(path)
		{
				mazeData = new Bitmap(path);	
		}
		
		/// <summary>
		/// This method returns a copy of the Bitmap maze as another Bitmap. It is so we can draw a path on the copy.
		/// </summary>
		/// <returns>A direct copy of the original Bitmap.</returns>
		public override dynamic getMazeCopy()
		{
			return mazeData.Clone(new Rectangle(0, 0, mazeData.Width, mazeData.Height), mazeData.PixelFormat);
		}
		
		/// <summary>
		/// This method is used to determine if a particular position is the end of the maze. In this case, the end
		/// of a maze is indicated by the color blue.
		/// </summary>
		/// <param name="position">The position to check.</param>
		/// <returns>True if the position is blue, false otherwise.</returns>
		public override bool isEnd(dynamic position)
		{
			Tuple<int, int> coords = Convert.ChangeType(position, typeof(Tuple<int, int>));
			if(determineColor(mazeData.GetPixel(coords.Item1, coords.Item2)) == PixelColors.Blue){
				return true;
			}
			return false;
		}
		
		/// <summary>
		/// This method is used to check if a particular position is a valid position. A valid position in this case is a position
		/// that is either white, blue, or red.
		/// </summary>
		/// <param name="position">The position to check.</param>
		/// <returns>True if the position is not black.</returns>
		public override bool validPosition(dynamic position){
			Tuple<int, int> coords = Convert.ChangeType(position, typeof(Tuple<int, int>));
			if(coords.Item1 >= 0 && coords.Item1 < mazeData.Width && coords.Item2 >= 0 && coords.Item2 < mazeData.Height){
				
				PixelColors c = determineColor(mazeData.GetPixel(coords.Item1, coords.Item2));
				if(c == PixelColors.White || c == PixelColors.Blue || c == PixelColors.Red){
					return true;
				}
			}
			return false;
		}
		
		/// <summary>
		/// This method is used to find both the start and end coordinates of a PixelMaze. These coordinates are necessary for the A* algorithm
		/// to function correctly.
		/// </summary>
		public void findStartEnd(){
			
			bool end = false;																	//flag for finding end position
			bool start = false;																	//flag for finding start position
			
			for(int i = 0; i < mazeData.Width; i++){
				for(int j = 0; j < mazeData.Height; j++){
					if(!start && determineColor(mazeData.GetPixel(i, j)) == PixelColors.Red){	//start found at first encountered red pixel
						startPos = new Tuple<int, int>(i, j);
						start = true;

					}
					if(!end && determineColor(mazeData.GetPixel(i, j)) == PixelColors.Blue){	//end found at first encountered blue pixel.
						endPos = new Tuple<int, int>(i, j);
						end = true;

					}
					if(start && end) return;													//return once both positions are found.
				}
			}
			
		}
		
		/// <summary>
		/// This method is used if code utilizing this class only wants the start.
		/// </summary>
		public override void findStart(){
			
			bool found = false;
			
			for(int i = 0; i < mazeData.Width; i++){
				for(int j = 0; j < mazeData.Height; j++){
					if(!found && determineColor(mazeData.GetPixel(i, j)) == PixelColors.Red){
						startPos = new Tuple<int, int>(i, j);
						found = true;

					}

				}
			}
			
		}
		
		/// <summary>
		/// This method is used to return a List of Tuples corresponding to the coordinates of the 4 pixels surrounding
		///  the pix Tuple passed in as an argument. This method only returns coordinates for the 4 cardinal directions.
		/// </summary>
		/// <param name="pos">The position to get surrounding positions for.</param>
		/// <returns></returns>
		public override dynamic getSurroundingPositions(dynamic pos){
			Tuple<int,int> pix = Convert.ChangeType(pos, typeof(Tuple<int, int>));
			
			List<Tuple<int, int>> surr = new List<Tuple<int, int>>();
			surr.Add(new Tuple<int, int>(pix.Item1 + 1, pix.Item2));
			surr.Add(new Tuple<int, int>(pix.Item1 - 1, pix.Item2));
			surr.Add(new Tuple<int, int>(pix.Item1, pix.Item2 + 1));
			surr.Add(new Tuple<int, int>(pix.Item1, pix.Item2 - 1));
			
			return surr;
		}
		
		/// <summary>
		/// This method is used to return all 8 positions surrounding a particular position. It returns a List of Tuples as well as a hashset of Tuples.
		/// The list contains all directions in this order {s, w, n, e, nw, ne, sw, se} while the HashSet only contains {nw, ne, sw, se}. This is to
		/// make processing more straightforward in the A* algorithm.
		/// </summary>
		/// <param name="pos">The position to check around.</param>
		/// <returns>A Tuple that contains the list containing all surrounding positions as well as a hashset containing only diagonal positions</returns>
		public Tuple<List<Tuple<int,int>>, HashSet<Tuple<int,int>>> getSurroundingEightEx(Tuple<int, int> pos){
			
			Tuple<List<Tuple<int,int>>, HashSet<Tuple<int,int>>> surr = new Tuple<List<Tuple<int, int>>, HashSet<Tuple<int, int>>>(new List<Tuple<int,int>>(), new HashSet<Tuple<int,int>>());
			surr.Item1.Add(new Tuple<int, int>(pos.Item1, pos.Item2 + 1));			//south
			surr.Item1.Add(new Tuple<int, int>(pos.Item1 - 1, pos.Item2));			//west
			surr.Item1.Add(new Tuple<int, int>(pos.Item1, pos.Item2 - 1));			//north
			surr.Item1.Add(new Tuple<int, int>(pos.Item1 + 1, pos.Item2));			//east
			surr.Item1.Add(new Tuple<int, int>(pos.Item1 - 1, pos.Item2 - 1));		//nw
			surr.Item2.Add(new Tuple<int, int>(pos.Item1 - 1, pos.Item2 - 1));
			surr.Item1.Add(new Tuple<int, int>(pos.Item1 + 1, pos.Item2 - 1));		//ne
			surr.Item2.Add(new Tuple<int, int>(pos.Item1 + 1, pos.Item2 - 1));
			surr.Item1.Add(new Tuple<int, int>(pos.Item1 - 1, pos.Item2 + 1));		//sw
			surr.Item2.Add(new Tuple<int, int>(pos.Item1 - 1, pos.Item2 + 1));
			surr.Item1.Add(new Tuple<int, int>(pos.Item1 + 1, pos.Item2 + 1));		//se
			surr.Item2.Add(new Tuple<int, int>(pos.Item1 + 1, pos.Item2 + 1));
			return surr;
			
		}
		
		/// <summary>
		/// This method is used to determine the color of a Color object under certain threshholds. This method is necessary
		/// in order to ensure that multiple shades of red, black, blue, and white are treated the same within a PixelMaze.
		/// It makes use of the Color classes GetHue and GetBrightness methods.
		/// </summary>
		/// <param name="c">The color to determine</param>
		/// <returns>A PixelColor member, either Black, White, Blue, or Red.</returns>
		public static PixelColors determineColor(Color c){
			float hue = c.GetHue();
			float brt = c.GetBrightness();
			
			if (brt < 0.2)  return PixelColors.Black;
			if (brt > 0.8)  return PixelColors.White;
			
			
			if (hue < 30)   return PixelColors.Red;
			if (hue < 270 && hue > 210)  return PixelColors.Blue;

			return PixelColors.Red;
		}
		
	}
}
