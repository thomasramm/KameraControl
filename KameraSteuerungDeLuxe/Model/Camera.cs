namespace KameraSteuerungDeLuxe.Model
{
    public class Camera(string name, string adresse)
    {
        public string Name { get; set; } = name;

        public string Address { get; set; } = adresse;

        public int Port { get; set; } = 5678;

        public override string ToString()
        {
            return $"{Name} ({Address})";
        }
    }
}