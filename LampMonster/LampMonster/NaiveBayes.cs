using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LampMonster
{
    class NaiveBayes
    {
       NaiveBayes(CategoryPaths paths)
        {
	        categories = vector<Category> (paths.size()); //TODO: Think over this...
	        int i = 0;
	        for(auto path = paths.cbegin(); path != paths.cend(); path++) //for each training path in paths
		    categories[i++] = Category(path->first,BuildBagOfWords(path->second));	//Build and fill a category with a bag of words
        }

        string Classify(List<string> words)
        {
	        string category = categories.begin()->first;	//Set first category as default
	        double maxProb = 0;						//Set current maximum category probability to 0
	        for(Category c : categories) {			//For each category
		        double curProb = (NBC(1/categories.size(), c.second, words, prior)); //Calculate probability for the words belonging to that category
		        if (curProb>maxProb) {				//If probability is higher that of the previous most probable (fittest) category
			        maxProb = curProb;				//Update maximums
			        category = c.first;
		        }	
	        }
	    return category;			//Returns the name of the most probable category
        }

        double NBC(double probOfClass, Map<string,int> vocabulary, List<string> words, double prior)
        {
	        double product = 1;
	        int totNumWordsInVoc = 0;

	        for(auto i = vocabulary.cbegin(); i != vocabulary.cend(); i++) //Get the sum of all words in the vocabulary map
		        totNumWordsInVoc += i->second;

	        double nom = (totNumWordsInVoc + vocabulary.size()*prior);
	        foreach (string word in words) //calculate probalility for each word to belong to classification with the vocabulary
	        {
		        double denom = prior;
		        if (vocabulary.find(word) != vocabulary.end())
			        denom += vocabulary.at(word);
		        product *= (denom/nom);
	        }
	        return product*probOfClass;
        }
}