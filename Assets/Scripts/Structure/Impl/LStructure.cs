namespace Structure.Impl {
    public class LStructure : Structure {
        public void Awake() {
            Grid = new byte[,] {
                { 0, 0, 0, 5 },
                { 0, 0, 0, 6 },
                { 0, 0, 0, 6 },
                { 2, 3, 3, 11 }
            };
        }
    }
}