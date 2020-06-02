using System;
using System.Windows.Threading;

namespace find_undercover_cop.Model.AI.NeutralNetwork
{
    public class OCRNetwork: BackPropagationRPROPNetwork
    {
        //member
        private readonly MainWindow owner;

        //ctor
        public OCRNetwork(MainWindow owner, int[] nodesInEachLayer) : base(nodesInEachLayer)
        {
            this.owner = owner;
        }

        //methods
        public int BestNodeIndex
        {
            get
            {
                var result = -1;
                double aMaxNodeValue = 0;
                var aMinError = double.PositiveInfinity;
                for (var i = 0; i < OutputNodesCount; i++)
                {
                    var node = OutputNode(i);
                    if ((node.Value > aMaxNodeValue) || ((node.Value >= aMaxNodeValue) && (node.Error < aMinError)))
                    {
                        aMaxNodeValue = node.Value;
                        aMinError = node.Error;
                        result = i;
                    }
                }
                return result;
            }
        }
        public override void Train(PatternsCollection patterns)
        {
            var iteration = 0;
            if (patterns != null)
            {
                double error = 0;
                var good = 0;

                // Train until all patterns are correct
                while (good < patterns.Count)
                {
                    error = 0;
                    //
                    //tutaj wyswietlania chyba nie bedziemy robic, wszystko do przemyslenia
                    //
                    //owner.progressBar.Dispatcher.Invoke(() => owner.progressBar.Value = good, DispatcherPriority.Background);
                    //owner.labelSTEP3.Content = "Training progress: " + Math.Round(good * 100 / owner.progressBar.Maximum, 2) + "%";
                    good = 0;
                    for (var i = 0; i < patterns.Count; i++)
                    {
                        for (var k = 0; k < NodesInLayer(0); k++)
                            nodes[k].Value = patterns[i].Input[k];
                        Run();
                        var idx = (int)patterns[i].Output[0];
                        for (var k = 0; k < OutputNodesCount; k++)
                        {
                            error += Math.Abs(OutputNode(k).Error);
                            if (k == idx)
                                OutputNode(k).Error = 1;
                            else
                                OutputNode(k).Error = 0;
                        }
                        Learn();
                        if (BestNodeIndex == idx)
                            good++;

                        iteration++;
                    }

                    foreach (var link in links) ((EpochBackPropagationLink)link).Epoch(patterns.Count);
                    //
                    //if ((iteration % 2) == 0)
                    //    owner.labelnr2STEP3.Content = "AVG Error: " + (error / OutputNodesCount) + "  Iteration: " + iteration;
                    //
                }

                //
                //aktualizacja labeli po wszystkim
                //
                //owner.progressBar.Value = good;
                //owner.labelSTEP3.Content = "Training progress: " + (good * 100 / owner.progressBar.Maximum) + "%";
                //owner.labelnr2STEP3.Content = "AVG Error: " + (error / OutputNodesCount) + "  Iteration: " + iteration;
            }
        }
    }
}
