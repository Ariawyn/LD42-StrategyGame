using UnityEngine;

// Struct that associates inputs from two different keys (and their alternatives) together
public struct InputAxis 
{
    // The set of keys being tied together
    KeyCode positive, negative;
    KeyCode alt_positive, alt_negative;

    public InputAxis(KeyCode positive, KeyCode negative, KeyCode alt_positive, KeyCode alt_negative) 
    {
        // Initialize the keys
        this.positive = positive;
        this.negative = negative;
        this.alt_positive = alt_positive;
        this.alt_negative = alt_negative;
    }

    public float GetInputRaw()
    {
        // Get total positive input
        bool totalPositiveInput = Input.GetKey(this.positive) || Input.GetKey(this.alt_positive);
        
        // Get total negative input
        bool totalNegativeInput = Input.GetKey(this.negative) || Input.GetKey(this.alt_negative);

        // Return appropriate values, ie. : -1, 0, 1.
        if(totalPositiveInput && !totalNegativeInput)
        {
            return 1;
        }
        else if(totalNegativeInput && !totalPositiveInput)
        {
            return -1;
        }
        else 
        {
            return 0;
        }
    }

}