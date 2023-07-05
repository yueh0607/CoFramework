namespace CoFramework.Tween
{
    public class StringSteper : TweenSteper<string>
    {

        private string longString = null;

        public string LongString
        {
            get
            {
                longString ??= ValueStart.Length > ValueEnd.Length ? ValueStart : ValueEnd;
                return longString;
            }
        }


        public override void MoveNext(float step)
        {
            int st = ValueStart.Length;
            int ed = ValueEnd.Length;

            int lt = Utility.LerpHelper.Lerp(st, ed, step);


            Current.Value = LongString.Substring(st, lt - st);


        }
    }
}