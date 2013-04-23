using System;
namespace LampMonster
{
    interface IPerceptron
    {
        string Category { get; }
        double Classify(Document document);

    }
}
