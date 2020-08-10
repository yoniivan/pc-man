public class Edge
{
    public Edge(float weight, Vertex startV, Vertex endV)
    {
        Weight = weight;
        StartV = startV;
        EndV = endV;
    }

    public float Weight { get; set; }
    public Vertex StartV { get; set; }
    public Vertex EndV { get; set; }
}
