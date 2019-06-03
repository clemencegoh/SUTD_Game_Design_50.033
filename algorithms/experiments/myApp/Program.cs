using System;
using System.Collections.Generic;
using System.Collections;
using System.Numerics;



namespace myApp
{
    class Program
    {
        static void Main(string[] args)
        {
            
            DungeonGeneration.Stage s = new DungeonGeneration.Stage(6, 2);

            DungeonGeneration.OverWorld overworld = new DungeonGeneration.OverWorld(s);

            // overworld.first_stage.PrintStage();  // prints the stage out

            // DungeonGeneration.Room r = overworld.first_stage.first_room;


        }
    }
}


namespace DungeonGeneration
{
    /* 
        Overworld class here is just a wrapper for a singly linked list

        Contains the overview for the entire series of stages
     */
    class OverWorld
    {
        public Stage first_stage;
        public Stage current_stage;

        public OverWorld(){}

        public OverWorld(Stage first_stage)
        {
            // This should be a pointer to the first stage
            this.first_stage = first_stage;
            this.current_stage = first_stage;
        }

        public Stage Next_Stage(){
            current_stage = current_stage.next;
            return current_stage;
        }

    }


    /*
        Stage class, or minimap, contains the overall rooms within a single stage
        
        2D array containing Rooms
     */
    class Stage
    {
        public Room[,] map;
        public Stage next;
        public Room first_room;

        // Default constructor, for placeholder Stage
        public Stage(){
            
            // Use Default of 4
            this.map = new Room[6, 6];

            GenerateStage(0);
        }

        public Stage(int min_nodes, int max_depth){
            // min number of nodes before boss room

            // Generate 2D array placeholder
            this.map = new Room[min_nodes+2, min_nodes+2];

            GenerateStage(max_depth);
        }

        // Generates the map of the entire stage
        public void GenerateStage(int max_depth)
        {
            // Set start and boss
            int[] positions = this.setInitialRooms();

            // Start Generating
            fillMainRooms(positions[0], positions[1]);

            // Fill side rooms
            // fillSideRooms(positions[0], 0, max_depth);
            
            this.PrintStage();
        }


        // Helper function for filling out side rooms
        private void fillSideRooms(Coords from, Coords current, int max_depth)
        {
            int max_len = this.map.GetLength(0) - 1;
            if (current.x > max_len || current.y > max_len)
            {
                return;
            }

            // TODO: This shit
            
            // Seems like a recursion problem
            checkUp(from, current, max_depth);
            checkRight(from, current, max_depth);
            checkDown(from, current, max_depth);
            checkLeft(from, current, max_depth);

            Coords next = new Coords(current.x + 1, current.y);
            fillSideRooms(current, next, max_depth);

        }

        // Recursive function for deciding 
        private void checkUp(int current_x, int current_y, int depth)
        {

        }
        private void checkRight(int current_x, int current_y, int depth)
        {

        }
        private void checkLeft(int current_x, int current_y, int depth)
        {

        }
        private void checkDown(int current_x, int current_y, int depth)
        {

        }


        // helper function for filling up main path rooms between start and end 
        private void fillMainRooms(int start, int end)
        {
             // Determine how many to generate
            int vector_right = this.map.GetLength(1) - 1;
            int vector_other;  // Length to go for other direction
            bool direction;  // False if down, True if up

            if (start < end)
            {
                // Start higher than end
                direction = false;
                vector_other = end - start;
            }else{
                direction = true;
                vector_other = start - end;
            }

            // Generate based on vectors
            // Create LinkedList for popping
            LinkedList<String> FillOrder = new LinkedList<String>();

            // Add right
            for (int i=0;i<vector_right + 1;i++)
            {
                FillOrder.AddLast("Right");
            }

            // Add other
            if (vector_other != 0)
            {
                Random random = new Random();
                int[] insert_positions = new int[vector_other + 1];
                for (int i=0;i<vector_other + 1;i++)
                {
                    // Generate list of positions to insert into
                    insert_positions[i] = random.Next(vector_right);
                }

                // Create List order to insert
                Array.Sort(insert_positions);

                LinkedListNode<String> current = FillOrder.First;
                for (int i=0; i<insert_positions[0]; i++)
                {
                    current = current.Next;
                }

                for (int i=1; i<vector_other + 1; i++)
                {
                    FillOrder.AddAfter(current, direction?"Up":"Down");

                    if (insert_positions[i] > insert_positions[i-1])
                    {
                        for (int j=0;j<insert_positions[i] - insert_positions[i-1];j++)
                        {
                            current = current.Next;
                        }
                    }
                }
            }
            
            LinkedListNode<String> curr = FillOrder.First;
            

            int current_x = start;
            int current_y = 0;

            curr = FillOrder.First;

            // Actually Insert
            while (curr.Next != null)
            {
                bool lastRoom = false;

                // check last case for boss room
                if (current_y == this.map.GetLength(0) - 1)
                {
                    if (current_x + 1 == end || current_x - 1 == end)
                    {
                        lastRoom = true;
                    }
                }
                
                if (current_y == this.map.GetLength(0) - 2)
                {
                    if (current_x == end)
                    {
                        lastRoom = true;
                    }
                }

                // create new room type based on last
                Room r;
                if (lastRoom)
                {
                    r = new BossRoom();
                }else{
                    r = new NormalRoom();
                }
                
                switch (curr.Value)
                {
                    case "Up":
                        this.map[current_x, current_y].Up = r;
                        r.Down = this.map[current_x, current_y];
                        current_x -= 1;
                        break;
                    case "Down":
                        this.map[current_x, current_y].Down = r;
                        r.Up = this.map[current_x, current_y];
                        current_x += 1;
                        break;
                    default:  // right
                        this.map[current_x, current_y].Right = r;
                        r.Left = this.map[current_x, current_y];
                        current_y += 1;
                        break;
                }
                this.map[current_x, current_y] = r;
                r.coords = new Coords(current_x, current_y);
                curr = curr.Next;
            }

        }

        // helper functions for generation of stage
        private int[] setInitialRooms()
        {
            int arrayLength = map.GetLength(1);

            Random random = new Random();
            int start_x = random.Next(arrayLength);
            int end_x = random.Next(arrayLength);

            StartRoom s = new StartRoom();
            s.coords = new Coords(start_x, 0);
            this.first_room = s;
            this.map[s.coords.x, s.coords.y] = s;

            /* BossRoom b = new BossRoom();
            b.coords = new Coords(end_x, arrayLength - 1); */
            // this.map[b.coords.x, b.coords.y] = b;

            int[] positions = new int[2];
            positions[0] = start_x;
            positions[1] = end_x;

            return positions;
        }

        public void PrintStage()
        {
            int length = this.map.GetLength(1);
            for (int i=0;i<length;i++)
            {
                for (int j=0;j<length;j++)
                {   
                    if (this.map[i, j] != null)
                    {
                        Console.Write("{0} \t", this.map[i, j].GetName());
                    }else{
                        Console.Write("- \t");
                    }
                   
                }
                Console.WriteLine("");
            }
        }


        // Setters
        public void SetMap(int x, int y, Room item){
            this.map[x, y] = item;
        }

        public void SetNextStage(Stage stage){
            this.next = stage;
        }

    }


    /*
        Class Room, basic building block for dungeong generation

        Abstract class, for all other types of Rooms present in the Dungeon

        Type of rooms:
        - Normal
        - Start (could share similar properties to normal)
        - Boss 
        - Shop
        - Reward/Treasure
     */
    public abstract class Room{

        public Coords coords;
        public Room Up;
        public Room Down;
        public Room Left;
        public Room Right;

        // Default constructor
        public Room(){}

        // For testing purposes, does something in the room
        public abstract int Do();

        public abstract string GetName();

        /* public Room GetNextRoom(string name)
        {
            if (this.Up?.GetName() == name)
            {
                return this.Up;
            }
            if (this.Right?.GetName() == name)
            {
                return this.Right;
            }
            if (this.Down?.GetName() == name)
            {
                return this.Down;
            }
            /* if (this.Left?.GetName() == name)
            {
                return this.Left;
            } */
            return null;
        } */
    }

    public struct Coords
    {
        public int x, y;

        public Coords(int p1, int p2)
        {
            x = p1;
            y = p2;
        }
    }

    public class StartRoom : Room
    {
        public StartRoom(){}

        public override string GetName()
        {
            return "Start";
        }

        public override int Do(){
            return 1;
        }

    }

    public class NormalRoom : Room
    {
        public NormalRoom(){}

        public override string GetName()
        {
            return "Normal";
        }

        public override int Do(){
            return 0;
        }
    }

    public class SideRoom: Room
    {
        public SideRoom(){}

        public override string GetName()
        {
            return "Normal";
        }

        public override int Do(){
            return 0;
        }
    }

    public class BossRoom : Room
    {
        public BossRoom(){}

        public override string GetName()
        {
            return "Boss";
        }
        
        public override int Do(){
            return -1;
        }
    }

    public class ShopRoom : Room
    {
        public ShopRoom(){}

        public override string GetName()
        {
            return "Shop";
        }

        public override int Do(){
            return 1;
        }
    }

    public class TreasureRoom : Room
    {
        public TreasureRoom(){}

        public override string GetName()
        {
            return "Treasure";
        }

        public override int Do(){
            return 2;
        }

    }


}
