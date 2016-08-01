using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;

public class SeaTile : MonoBehaviour
{
    public class Position {
        public Vector2 world;
        public Vector3 screen;

        public static List<List<SeaTile.Position>> SplitPath(
                List<SeaTile.Position> stack,
                List<SeaTile.Position> path,
                List<SeaTile.Position> seenPositions)
        {
            List<List<SeaTile.Position>> split = new List<List<SeaTile.Position>>();

            foreach (SeaTile.Position neighbour in path[path.Count - 1].GetNeighbours(stack)) {
               if (! seenPositions.Contains(neighbour)) {
                   List<SeaTile.Position> newPath = new List<SeaTile.Position>(path);
                   newPath.Add(neighbour);
                   split.Add(newPath);
                   seenPositions.Add(neighbour);
               }
            }

            return split;
        }

        public static List<SeaTile.Position> FindPath(
                List<SeaTile.Position> stack,
                SeaTile.Position position,
                SeaTile.Position target)
        {
            List<SeaTile.Position> foundPath = null;
            List<SeaTile.Position> seenPositions = new List<SeaTile.Position>();
            List<List<SeaTile.Position>> possibilities = new List<List<SeaTile.Position>>();

            foreach (SeaTile.Position first in position.GetNeighbours(stack)) {
               List<SeaTile.Position> possibility = new List<SeaTile.Position>();

               possibility.Add(first);
               possibilities.Add(possibility);
               seenPositions.Add(first);
            }

            int index = 0;

            while (foundPath == null && index < 50 * 50) {
                List<List<SeaTile.Position>> newPossibilities = new List<List<SeaTile.Position>>();

                foreach (List<SeaTile.Position> possibility in possibilities) {
                    
                    foreach(List<SeaTile.Position> splitted in SplitPath(stack, possibility, seenPositions)) {
                        SeaTile.Position last = splitted[splitted.Count -1];
                        
                        if (last.world.x == target.world.x && last.world.y == target.world.y) {
                            foundPath = splitted;
                            break;
                        }

                        newPossibilities.Add(splitted);
                    }
                
                }

                possibilities = newPossibilities;
                index++;
            }

            if (foundPath == null) throw new System.Exception("No path found in 50x50 iterations : make sure SeaTiles have a generated position");

            return foundPath;
        }

        public Position(SeaTile tile) {
            this.world = tile.worldPosition;
            this.screen = tile.rectTransform.localPosition;
        }

        public List<SeaTile.Position> GetNeighbours(List<SeaTile.Position> stack) {
            List<SeaTile.Position> neighbours = new List<SeaTile.Position>();            

            SeaTile.Position top = stack.Find(p => p.world.x == world.x && p.world.y == world.y + 1);
            SeaTile.Position right = stack.Find(p => p.world.x == world.x + 1 && p.world.y == world.y);
            SeaTile.Position bottom = stack.Find(p => p.world.x == world.x && p.world.y == world.y - 1);
            SeaTile.Position left = stack.Find(p => p.world.x == world.x - 1 && p.world.y == world.y);

            if (top != null) neighbours.Add(top);
            if (right != null) neighbours.Add(right);
            if (bottom != null) neighbours.Add(bottom);
            if (left != null) neighbours.Add(left);

            return neighbours;
        }
    }

    [System.Serializable]
    public class Vector3Event: UnityEvent<Vector3> {}

    public RectTransform rectTransform;
    public Game game;
    public Vector2 worldPosition;

    public Position position;

    void Start() {
        position = new Position(this);
        game.FeedSeaTileStack(position);
    }

    public void HandleTileClicked()
    {
        game.MoveCat(position);
    }
}
