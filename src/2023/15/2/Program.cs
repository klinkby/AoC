// Read the input text file and parse sequences.
// Move lenses in and out of boxes.
// Summarize the box index * lens index * focal Length. 

var reg = new Regex(@"(?'seq'(?'label'\w+)=?(?'lens'-|\d+))(,|$)",
    RegexOptions.Compiled | RegexOptions.ExplicitCapture);
var input = File.ReadLines("../input.txt").First();
var sequence =
    from m in reg.Matches(input)
    let lensLabel = m.Groups["label"].Value
    let boxIndex = lensLabel.Aggregate(0, (a, b) => (a + b) * 17 % 256)
    let lens = m.Groups["lens"].Value
    let focalLength = "-" == lens ? -1 : int.Parse(lens) // -1 means remove
    select (boxIndex, lensLabel, focalLength);

var boxes = new List<(int boxIndex, string lensLabel, int focalLength)>?[256];
foreach (var step in sequence)
{
    var box = boxes[step.boxIndex] ??
              (boxes[step.boxIndex] = new List<(int boxIndex, string lensLabel, int focalLength)>());
    if (-1 == step.focalLength) // remove
    {
        box.RemoveAll(x => x.lensLabel == step.lensLabel);
    }
    else
    {
        var pos = box.FindIndex(x => step.lensLabel == x.lensLabel);
        if (-1 == pos) // not found
            box.Add(step);
        else
            box[pos] = step;
    }
}

var sum = (
    from boxIndex in Enumerable.Range(0, boxes.Length)
    let box = boxes[boxIndex]
    where null != box
    from lensIndex in Enumerable.Range(0, box.Count)
    let lens = box[lensIndex]
    select (1 + boxIndex) * (1 + lensIndex) * lens.focalLength
).Sum();

Debug.Assert(269410 == sum);
Console.WriteLine(sum);