using System.Collections;

public interface IAnimated
{
    IEnumerator GetAnimationNames();
}

public interface ISounded
{
    IEnumerator GetSoundNames();
}