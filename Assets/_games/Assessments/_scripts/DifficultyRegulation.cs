using System;
using UnityEngine;

/// <summary>
/// Evenly divides the difficulty range to arrange parameters base with difficulty
/// Note that this may not be desired if difficulty is pre-determined to  have
/// only few discrete values!
/// 
/// Increase( 1, 5)
/// 
/// 0                                                                        1
/// difficulty---------------------------------------------------------------->
///            1    |         2        |        3         |        4         |   (no offset)
///   1    |         2        |         3         |        4         |        5  (offset)
/// 1                2                  3                  4                  5
///
/// Using some offsett we can get maximum value even if difficulty is slightly less than 1
/// 
/// </summary>
public class DifficultyRegulation : MonoBehaviour {

    float difficulty;
    public DifficultyRegulation( float difficulty)
    {
        this.difficulty = difficulty;
    }

	public int Increase( int min, int max)
    {
        if (min > max)
            throw new ArgumentException( "This parameter should only increase.");

        return (int)Mathf.Lerp(min, max, difficulty);
    }

    public int Decrease( int max, int min)
    {
        if (min > max)
            throw new ArgumentException( "This parameter should only decrease.");

        return (int)Mathf.Lerp (max, min, difficulty);
    }
}