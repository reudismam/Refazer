using System.Collections.Generic;
using ExampleRefactoring.Spg.ExampleRefactoring.Expression;
using ExampleRefactoring.Spg.ExampleRefactoring.Setting;
using Spg.ExampleRefactoring.Synthesis;

namespace ExampleRefactoring.Spg.ExampleRefactoring.Comparator
{
    /// <summary>
    /// Selection manager
    /// </summary>
    public class SelectorManager
    {
        /// <summary>
        /// Learned model
        /// </summary>
        readonly Dictionary<FeatureType, float> model = new Dictionary<FeatureType, float>();

        /// <summary>
        /// Consider empty string
        /// </summary>
        /// <returns>True is consider empty string</returns>
        //public Boolean considerEmpty { get; set; }
        public SynthesizerSetting Setting { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="setting">Setting</param>
        public SelectorManager(SynthesizerSetting setting) {
            this.Setting = setting;
        }

        /// <summary>
        /// Order expression
        /// </summary>
        /// <param name="solution"></param>
        /// <returns>Ordered expression list</returns>
        public float Order(List<IExpression> solution) {

            InitModel();
            Dictionary<FeatureType, int> features = SynthesisManager.CreateInstance(solution);

            float relevance = 0.0f;
            foreach (KeyValuePair<FeatureType, int> entry in features)
            {
                relevance += entry.Value * model[entry.Key];
            }

            return relevance;
        }

        /// <summary>
        /// Order
        /// </summary>
        /// <param name="expression">Order expression</param>
        /// <returns>Ordered expression</returns>
        public float Order(IExpression expression)
        {
            List<IExpression> solution = new List<IExpression> {expression};
            InitModel();
            Dictionary<FeatureType, int> features = SynthesisManager.CreateInstance(solution);

            float relevance = 0.0f;
            foreach (KeyValuePair<FeatureType, int> entry in features)
            {
                relevance += entry.Value * model[entry.Key];
            }

            return relevance;
        }

        /// <summary>
        /// Init model
        /// </summary>
        private void InitModel()
        {

            model.Add(FeatureType.CONSTSTR, 0.165861f);
            model.Add(FeatureType.POS, 0.154214f);
            //model.Add(HypothesisManager.CPOSBEGIN, 0.316224f);
            model.Add(FeatureType.CPOSBEGIN, 0.1f);
            model.Add(FeatureType.CPOSEND, 3.504379f);
            model.Add(FeatureType.CPOS, -0.463805f);
            model.Add(FeatureType.SYNTAX, 0.5f);
            model.Add(FeatureType.SIZE, 0);

            if (Setting.ConsiderEmpty)
            {
                model.Add(FeatureType.EMPTY, 100);
            }
            else {
                model.Add(FeatureType.EMPTY, 0);
            }

            if (Setting.DynamicTokens)
            {
                model.Add(FeatureType.DYMTOKEN, 1000);
            }
            else
            {
                model.Add(FeatureType.DYMTOKEN, 0);
            }
        }
    }
}
