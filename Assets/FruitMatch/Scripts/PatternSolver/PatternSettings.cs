using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu]
public class PatternSettings : ScriptableObject
{
 [SerializeField] public List<Pattern> allPatterns = new List<Pattern>();
 [SerializeField] public List<PatternSetup> patternSetup = new List<PatternSetup>();
 
}

[Serializable] public struct PatternRelations
{
 public Pattern allPatternsExceptThisPattern;
 public bool canFunctionTogether;
 public PatternRelations(Pattern allPatternsExceptThisPattern, bool canFunctionTogether)
 {
  this.allPatternsExceptThisPattern = allPatternsExceptThisPattern;
  this.canFunctionTogether = canFunctionTogether;
 }
}

[Serializable] public struct PatternSetup
 {
  public Pattern thisPattern;
  public bool currentlyActive;
  public bool isAllowed;
  public List<PatternRelations> relationToOtherPatterns;

  public PatternSetup(Pattern thisPattern, bool currentlyActive, bool isAllowed,
   List<PatternRelations> relationToOtherPatterns)
  {
   this.thisPattern = thisPattern;
   this.currentlyActive = currentlyActive;
   this.isAllowed = isAllowed;
   this.relationToOtherPatterns = relationToOtherPatterns;
  }
 }

