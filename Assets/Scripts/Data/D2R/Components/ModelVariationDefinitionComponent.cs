using UnityEngine;
using SimpleJSON;
using System.Collections.Generic;

namespace Diablo2Editor
{
    /*
    * Instances 1 random model from list
    * 
    */
    public class ModelVariationDefinitionComponent : LevelEntityComponent
    {
        public List<ModelVariationComponent> variations;
        public override void Deserialize(JSONObject json)
        {
            base.Deserialize(json);
            variations = ISerializable.DeserializeList<ModelVariationComponent>(json, "variations");
        }

        public override JSONObject Serialize()
        {
            JSONObject result = base.Serialize();
            result["variations"] = ISerializable.SerializeList(variations);
            return result;
        }

        public int GetRandomWeightedIndex(int seed)
        {
            // Get the total sum of all the weights.
            float weightSum = 0.0f;
            int elementCount = variations.Count;
            foreach (var variant in variations)
            {
                weightSum += variant.weight;
            }

            // Step through all the possibilities, one by one, checking to see if each one is selected.
            int index = 0;
            int lastIndex = elementCount - 1;
            while (index < lastIndex)
            {
                // Do a probability check with a likelihood of weights[index] / weightSum.
                if (Random.Range(0, weightSum) < variations[index].weight)
                {
                    return index;
                }

                // Remove the last item from the sum of total untested weights and try again.
                weightSum -= variations[index++].weight;
            }

            // No other item was selected, so return very last index.
            return index;
        }

        public override void Instantiate()
        {
            base.Instantiate();
            // using seed to get determinated result
            var preset = GetPreset() as LevelPreset;

            int seed = 0;
            if (preset != null)
            {
                seed = preset.seed;
            }
            Random.InitState(seed);
            if (variations.Count > 0)
            {
                int index = GetRandomWeightedIndex(seed);
                ModelDefinitionComponent model = gameObject.AddComponent<ModelDefinitionComponent>();
                model.entity = this.entity;
                model.Deserialize(variations[index].model_data);
                model.Instantiate();
            }
        }
    }
}