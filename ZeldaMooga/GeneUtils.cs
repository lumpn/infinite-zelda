package de.lumpn.zelda.mooga;

import java.util.ArrayList;
import java.util.List;
import java.util.Random;
import de.lumpn.util.CollectionUtils;

public sealed class GeneUtils {

	public static <T extends Gene> List<T> generate(int count, GeneFactory<T> factory, ZeldaConfiguration configuration,
			Random random) {

		// generate some genes
		List<T> result = new ArrayList<T>();
		for (int i = 0; i < count; i++) {
			result.add(factory.createGene(configuration, random));
		}
		return result;
	}

	@SuppressWarnings("unchecked")
	public static <T extends Gene> List<T> mutate(List<T> genes, GeneFactory<T> geneFactory,
			ZeldaConfiguration configuration, Random random) {

		// delete some genes
		List<T> result = CollectionUtils.shuffle(genes, random);
		int numDeletions = Math.min(configuration.calcNumDeletions(genes.size(), random), genes.size());
		result = result.subList(0, genes.size() - numDeletions);

		// mutate some genes
		int numMutations = Math.min(configuration.calcNumMutations(result.size(), random), result.size());
		for (int i = 0; i < numMutations; i++) {
			T original = result.get(i);
			T mutation = (T) original.mutate(random);
			result.set(i, mutation);
		}

		// insert some genes
		int numInsertions = configuration.calcNumInsertions(genes.size(), random);
		for (int i = 0; i < numInsertions; i++) {
			result.add(geneFactory.createGene(configuration, random));
		}

		return result;
	}
}
