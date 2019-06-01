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
            // Console.WriteLine("Hello World!");
            DungeonGeneration.Stage s = new DungeonGeneration.Stage(4);
            s.PrintStage();  // prints the stage out

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

        // Default constructor, for placeholder Stage
        public Stage(){
            
            // Use Default of 4
            this.map = new Room[6, 6];

            GenerateStage();
        }

        public Stage(int min_nodes){
            // min number of nodes before boss room

            // Generate 2D array placeholder
            this.map = new Room[min_nodes+2, min_nodes+2];

            GenerateStage();
        }

        // Generates the map of the entire stage
        public void GenerateStage()
        {
            // Set start and boss
            int[] positions = this.setInitialRooms();

            // Start Generating
            fillMainRooms(positions[0], positions[1]);
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

            // Console.WriteLine("move {0} right, move {1} {2}", vector_right, vector_other, direction ? "Up" : "Down");

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

                // Debug
                // Console.WriteLine("[{0}]", string.Join(", ", insert_positions));

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

            FillOrder.RemoveLast();
            
            LinkedListNode<String> curr = FillOrder.First;
            
            /* while(curr.Next !=null)
            {
                Console.Write("{0}\t", curr.Value);
                curr = curr.Next;
            }
            Console.WriteLine(); */


            int current_x = start;
            int current_y = 0;

            curr = FillOrder.First;

            // Actually Insert
            while (curr.Next != null)
            {
                switch (curr.Value)
                {
                    case "Up":
                        current_x -= 1;
                        break;
                    case "Down":
                        current_x += 1;
                        break;
                    default:  // right
                        current_y += 1;
                        break;
                }
                this.map[current_x, current_y] = new NormalRoom();
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

            BossRoom b = new BossRoom();
            b.coords = new Coords(end_x, arrayLength - 1);

            this.map[s.coords.x, s.coords.y] = s;
            this.map[b.coords.x, b.coords.y] = b;

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

        // Default constructor
        public Room(){}

        // For testing purposes, does something in the room
        public abstract int Do();

        public abstract string GetName();
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
