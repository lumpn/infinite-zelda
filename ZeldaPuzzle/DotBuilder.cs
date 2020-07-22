package de.lumpn.zelda.puzzle;

import java.io.PrintStream;

public class DotBuilder {

	public DotBuilder() {
		this.out = System.out;
	}

	public DotBuilder(PrintStream out) {
		this.out = out;
	}

	public void begin() {
		out.println("digraph {");
	}

	public void end() {
		out.println("}");
	}

	public void addNode(int id) {
		out.format("loc%d [label=\"%d\"];\n", id, id);
	}

	public void addEdge(int start, int end, String label) {
		out.format("loc%d -> loc%d [label=\"%s\"];\n", start, end, label);
	}

	private final PrintStream out;
}
