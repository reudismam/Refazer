using System;
using System.Collections.Generic;
using Spg.ExampleRefactoring.Expression;
using DiGraph;

namespace Spg.ExampleRefactoring.Digraph
{
    /// <summary>
    /// Class used to represent the set of mapping present in a given string
    /// </summary>
    public class Dag
    {
        /// <summary>
        /// Vertex mapping
        /// </summary>
        /// <returns>Vertex mapping</returns>
        public Dictionary<string, Vertex> vertexes { get; set;}

        /// <summary>
        /// Directed Graph with the nodes and connection of indexes present on the string
        /// </summary>
        public  DirectedGraph dag { get; set; }

        /// <summary>
        /// Set of mapping used to construct the synthesizer.
        /// </summary>
        public Dictionary<Tuple<Vertex, Vertex>, List<IExpression>> mapping { get; set; }

        /// <summary>
        /// First vertex
        /// </summary>
        /// <returns>First vertex</returns>
        public Vertex init { get; set; } 

        /// <summary>
        /// End node
        /// </summary>
        /// <returns>End node</returns>
        public Vertex end { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="dag">Directed graph</param>
        /// <param name="init">Start node</param>
        /// <param name="end">End node</param>
        /// <param name="mapping">Mapping</param>
        /// <param name="vertices">Vertexes</param>
        public Dag(DirectedGraph dag, Vertex init, Vertex end,  Dictionary<Tuple<Vertex, Vertex>, List<IExpression>> mapping, Dictionary<string, Vertex> vertices)
        {
            this.dag = dag;
            this.init = init;
            this.end = end;
            this.mapping = mapping;
            this.vertexes = vertices;
        }
    }
}
