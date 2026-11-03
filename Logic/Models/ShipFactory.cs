namespace Logic
{
    public static class ShipFactory
    {
        //I have added this to extend the game later by simply creating new ship class.
        public static Ship CreateShip(int length)   //this creates ship based on the length provided
        {
            return length switch
            {
                2 => new Destroyer(),        //using the switch case, already defined classes are instantiated here for creating ship
                3 => new Submarine(),
                _ => throw new ArgumentException()
            };
        }
    }
}