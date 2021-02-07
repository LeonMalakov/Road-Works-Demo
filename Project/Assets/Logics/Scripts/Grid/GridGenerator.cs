using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Combine
{
    public class GridGenerator
    {
        // GameConfig reference.
        private GameConfigData _gameConfig;

        // Lead line.
        private int[] _lead;
        private int[] _lastLead;

        // How many lines was generated.
        private int _linesCounter;

        public GridGenerator(GameConfigData config)
        {
            // Get GameConfig referece from parent object.
            _gameConfig = config;

            // Init lead arrays.
            _lead = new int[_gameConfig.GridSettings.LeadLinesCount];
            _lastLead = new int[_gameConfig.GridSettings.LeadLinesCount];

            // Set lead index is center.
            for (int i = 0; i < _gameConfig.GridSettings.LeadLinesCount; i++)
            {
                _lead[i] = _gameConfig.GridSettings.TotalLinesCount / 2;
                _lastLead[i] = _lead[i];
            }
        }

        /// <summary>
        /// Returns next sequence of CellType-s for next row. If 'isInitial', middle line will be empty.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<CellType> Next(bool isInitial = false)
        {
            int leftPlayableIndex = _gameConfig.GridSettings.TotalLinesCount / 2 - _gameConfig.GridSettings.PlayableLinesCount / 2;
            int rightPlayableIndex = _gameConfig.GridSettings.TotalLinesCount / 2 + _gameConfig.GridSettings.PlayableLinesCount / 2;

            // Index of lead line where obstacle will be placed.
            // -1, if it is no obstacle.
            int obstaclePathIndex = ResolveObstaclePathIndex();

            for (int i = 0; i < _gameConfig.GridSettings.TotalLinesCount; i++)
            {
                if (i >= leftPlayableIndex && i <= rightPlayableIndex)
                {
                    // Playable cells.

                    yield return ResolvePlayable(i, obstaclePathIndex);
                }
                else if (i == leftPlayableIndex - 1 || i == rightPlayableIndex + 1)
                {
                    // Borders around playable cells (exclusive).

                    yield return CellType.Border;
                }
                else
                {
                    // Non-playable cells.

                    yield return NonPlayableRandomCell();
                }
            }

            // Move lead lines.
            if (!isInitial)
                MoveLeads(leftPlayableIndex, rightPlayableIndex);

            // Increase lines counter.
            _linesCounter++;
        }

        // Returns -1, if it is no obstacle. Else, returns lead line index.
        private int ResolveObstaclePathIndex()
        {
            if (_linesCounter >= _gameConfig.GridSettings.ObstacleOccurrenceFrequency &&
                _linesCounter % _gameConfig.GridSettings.ObstacleOccurrenceFrequency == 0)
                return Random.Range(0, _gameConfig.GridSettings.LeadLinesCount);

            return -1;
        }

        private CellType ResolvePlayable(int i, int obstaclePathIndex)
        {
            // It is one of lead lines.
            if (_lead.Contains(i))
            {
                // It is line without obstacle.
                if (obstaclePathIndex == -1 || _lead[obstaclePathIndex] != i)
                    return CellType.Empty;
                // It is line where obstacle will be placed.
                else
                    return CellType.Obstacle;

            }
            // It is one of last lead lines.
            else if (_lastLead.Contains(i))
            {
                return CellType.Empty;
            }
            // It is not lead line.
            else
            {
                return CellType.NotWalkable;
            }
        }

        private void MoveLeads(int leftPlayableIndex, int rightPlayableIndex)
        {
            // Save lead as lastLead and Move lead index by -1, 0, or 1.
            for (int i = 0; i < _gameConfig.GridSettings.LeadLinesCount; i++)
            {
                _lastLead[i] = _lead[i];
                _lead[i] += Random.Range(_lead[i] != leftPlayableIndex ? -1 : 0, _lead[i] != rightPlayableIndex ? 2 : 1);
            }
        }

        private CellType NonPlayableRandomCell()
        {
            return (Random.Range(0, 100) < 20) ? CellType.Empty : CellType.NotWalkable;
        }
    }
}