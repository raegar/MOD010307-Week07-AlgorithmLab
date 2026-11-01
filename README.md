# Reef Rush + Algorithm Lab

The Week 7 practice project for **MOD010307 Mobile Game Programming**: searching, sorting and algorithmic thinking in Unity 6.3.

This is the Week 6 Reef Rush project with one new scene, the **Algorithm Lab** (`Assets/AlgorithmLab`). A parade of fish of different sizes, a 1,000-entry world leaderboard and a benchmark show what searching and sorting algorithms do, and how much work each one takes.

> **Practice, not assessment.** Use the *techniques* in your own microgames, but don't submit these scripts, or lightly altered copies of them, as Learning Outcome 3 evidence.

## Getting the starter

`Starter/AlgorithmLab_Starter.unitypackage` is the scene as it is at step A0: everything drawn and wired up, with the six algorithm methods left for you to write. Import it with **Assets > Import Package > Custom Package…**. It only needs a Unity 6 project made from the Universal 2D template (it brings its own sprites, prefab and scripts), so it works in Reef Rush or any other project.

## Controls

| Tab | Button | Does |
|---|---|---|
| Parade | **Shuffle** / **Nearly sorted** / **Reversed** | Rearrange the ten fish |
| | **Find** | Linear search for the next kind of fish (Blue, Green, … in turn) |
| | **Biggest** | Best-so-far search for the biggest fish |
| | **Selection sort** / **Insertion sort** | Sort smallest to biggest, animated; the table at the top keeps the last checks / swaps for each kind of data |
| Leaderboard | type a score, **Linear search** / **Binary search** | The rank that score would get, and how many checks it took |
| | **Submit** | Insert the score at the place binary search found (no re-sort) |
| Benchmark | **Run benchmark** | Time every algorithm at N = 1,000, 10,000 and 100,000 |

## The algorithms

| Method | Script | Algorithm | Big O |
|---|---|---|---|
| `IndexOfKind` | `FishParade` | Linear search, stopping at the first match; -1 if not found | O(n) |
| `IndexOfBiggest` | `FishParade` | Best so far | O(n) |
| `SelectionSort` | `FishParade` | Selection sort (coroutine, so you can watch it) | O(n²) always |
| `InsertionSort` | `FishParade` | Insertion sort (swap-based, stable) | O(n²) worst, O(n) on sorted data |
| `LinearRank` | `Leaderboard` | Linear search for a new score's place | O(n) |
| `BinaryRank` | `Leaderboard` | Binary search for a new score's place (lower bound); ties go below | O(log n) |
| `Benchmark` | `Benchmark` | The same algorithms on `List<int>`, timed with `Stopwatch`, plus `List.Sort` | |

`LabFish` and the rest of `FishParade`, `Leaderboard` and `Benchmark` are the starter: drawing, buttons and checks.

## Commit map

One commit per tutorial step. The Week 6 history comes first (it's the same project).

| Step | Commit message |
|---|---|
| A0 | Add the Algorithm Lab starter: fish parade, leaderboard and benchmark tabs, algorithms still to write |
| A1 | Search the parade: linear search for a kind of fish, and best-so-far for the biggest |
| A2 | Sort the parade with selection sort, counting checks and swaps |
| A3 | Add insertion sort, to compare with selection sort on different data |
| A4 | Find a new score's rank on the 1,000-entry leaderboard with linear and binary search, then insert it |
| B1 | Add a benchmark: time each algorithm at N = 1,000, 10,000 and 100,000 |
| Friday | Add Friday's algorithm drills (`AlgorithmDrills.cs`: these contain bugs on purpose) |
| — | Document the Algorithm Lab (this README) |

## Manual tests

| Test | Expected |
|---|---|
| Find, pressed six times | Each kind is found at its first index, or "not found (-1)" after 10 checks |
| Biggest | The largest size is highlighted; always 9 checks with 10 fish |
| Selection sort on any data | Sorted smallest to biggest; 45 checks |
| Insertion sort on already sorted data | 9 checks, 0 swaps |
| Insertion sort on reversed data | 45 checks, 45 swaps |
| Leaderboard: 54321, both searches | Rank 446; linear 446 checks, binary 10 |
| Leaderboard: 999999 | Rank 1 with both searches |
| Leaderboard: 0 | The very bottom with both searches |
| Leaderboard: a score already on the board | Both searches put it just below the existing one |
| Submit | "Still in order? Yes", and the new entry is highlighted |
| Benchmark | Linear search ×10 per row, selection and insertion sort about ×100, binary search almost flat |

## Credits

Fish sprites: [Fish Pack](https://kenney.nl/assets/fish-pack) by Kenney (CC0).

---

## Reef Rush (Week 6)

The Week 6 practice project for **MOD010307 Mobile Game Programming**: collections and game systems in Unity 6.3.

Fish swim across a coral reef in waves. Tap a fish to catch it, catch the fish on the target card for a bonus, and don't let ten fish get away. At the end, put your initials on the saved high score table.

> **Practice, not assessment.** Reef Rush is a guided practice project. Use its *techniques* in your own microgames, but don't submit these scripts, or lightly altered copies of them, as Learning Outcome 3 evidence.

### Controls

| Input | Does |
|---|---|
| Tap (or click) a fish | Catch it |
| **Swarm +500** (Ready screen) | Add 500 fish to play with |
| **Start** | Clear the reef and start wave 1 |
| Initials + **Save** (game over) | Add your score to the high score table |
| **Replay** | A fresh game |

### The collections

| Collection | Where | What it holds | What happens to it |
|---|---|---|---|
| `FishInfo[] catalogue` | `FishSpawner` | every kind of fish: type, sprite, points | set in the Inspector |
| `List<Fish> allFish` | `FishSpawner` | every fish on the reef | `Add` on spawn; removed (looping backwards) when caught or when it swims away; its `Count` ends each wave |
| `List<FishType> targets` | `TargetCard` | the fish still to catch on the card | `Remove` when one is caught; dealt again when empty |
| `Image[] icons` | `TargetCard` | the card's icons | kept in step with `targets` by index |
| `Wave[] waves` | `WaveRunner` | each wave's fish count, interval and speed | set in the Inspector; played in order by a coroutine |
| `List<ScoreEntry> entries` | `HighScoreTable` | the high scores | `Add`, `Sort` highest first (with a tie-breaker), `RemoveRange` to keep 10, saved as JSON in `PlayerPrefs` |

### Scripts

| Folder | Scripts |
|---|---|
| `Assets/Scripts/Fish` | `Fish`, `FishType`, `FishInfo`, `FishSpawner`, `TapCatcher`, `SelfSwimmer` (stress test only) |
| `Assets/Scripts/Scores` | `ScoreKeeper`, `TargetCard`, `ScoreEntry`, `HighScoreTable` |
| `Assets/Scripts/Waves` | `Wave`, `WaveRunner` |
| `Assets/Scripts/Game` | `GameManager` (Ready → Playing → Over) |
| `Assets/Scripts/UI` | `FrameRateDisplay` |
| `Assets/Scripts/Drills` | `CollectionDrills` (Friday's exercises: these contain bugs on purpose) |

### Commit map

One commit per tutorial step, so you can compare your project with each step.

| Step | Commit message |
|---|---|
| — | Create Reef Rush project from the Universal 2D template |
| A0 | Set up the Reef scene: Kenney Fish Pack sprites, seabed, seaweed and a Fish prefab |
| A1 | Spawn fish with a for loop, each with a random sprite from an array |
| A2 | Keep every fish in a List, add a 500-fish Swarm button, and move them all in one loop |
| A3 | Catch fish by tapping, and remove fish that swim off the reef (looping backwards) |
| A4 | Add fish types with points, a ScoreKeeper, and a target card kept in a List |
| B1 | Send the fish in data-driven waves, using a Wave array and a coroutine |
| B2 | Add a high score table: sort descending with a tie-breaker, keep the top 10, save as JSON |
| B3 | Add a GameManager: Ready, Playing and Over states, escapes, and initials for the high score table |
| B4 ⭐ | Add a stress test: a frame rate display and a per-fish Update toggle for the Profiler |
| B5 | Document Reef Rush: collections, controls, commit map, manual tests and credits (this README) |
| Friday | Add Friday's collection drills |

### Manual tests

| # | Test | Expected |
|---|---|---|
| 1 | Open the game, press Swarm | 500 fish appear; the counter shows them |
| 2 | Press Start | The swarm is cleared, "Wave 1 of 4" shows, then fish swim in from both sides |
| 3 | Tap a fish | It disappears; the score goes up by its points |
| 4 | Tap a fish shown on the target card | Extra 25 points; its icon leaves the card |
| 5 | Catch all three targets | Extra 100 points; a new card is dealt |
| 6 | Tap empty water | Nothing happens |
| 7 | Let fish swim away | Escaped counts up; at 10 the game ends with "Too many got away!" |
| 8 | Clear all four waves | "Reef cleared!" |
| 9 | Type initials and press Save | The score appears in the right place, highlighted; only the top 10 are kept |
| 10 | Close the app and open it again | The high scores are still there |
| 11 | Press Replay | A fresh game: score 0, Escaped 0, Ready screen |
| 12 | Stress test: Swarm Size 5000, Use Manager Loop on, then off | Compare BehaviourUpdate in the Profiler (CPU Usage, Hierarchy view) |

### Credits

- Sprites: [Fish Pack](https://kenney.nl/assets/fish-pack) by Kenney (www.kenney.nl), licensed [CC0](https://creativecommons.org/publicdomain/zero/1.0/).
- Font: Unity's built-in LegacyRuntime font.
