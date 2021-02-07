namespace Combine
{
    public struct GridCoordinates
    {
        /// <summary>
        /// X pos.
        /// </summary>
        public int Line;

        /// <summary>
        /// Z pos.
        /// </summary>
        public float Row;

        public GridCoordinates(int line, float row)
        {
            Line = line;
            Row = row;
        }
    }
}