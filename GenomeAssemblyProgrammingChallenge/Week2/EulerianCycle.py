#Uses python3

firstLine = input().split()
n = int(firstLine[0])
m = int(firstLine[1])
edges = []
for i in range(m):
	edges.append(list(map(int, input().split())))
adj = [[] for _ in range(n)]
number = 0
for (a, b) in edges:
	adj[a - 1].append(((b - 1), number))
	number += 1

degree = [0]*n
for a, b in edges:
	degree[a-1]+=1
	degree[b-1]-=1
if any(degree):
	print(0)
else:
	path = [0]
	visitTracker = set()
	while len(visitTracker)<len(edges):
		for i, point in enumerate(path):
			allVisited = True
			for nextPoint in adj[point]:
				if nextPoint[1] not in visitTracker:
					allVisited = False
					break
			if allVisited: continue
			newCycle = [point]
			current = point
			findNext = True
			while findNext:
				findNext = False
				for nextPoint in adj[current]:
					if nextPoint[1] not in visitTracker:
						visitTracker.add(nextPoint[1])
						newCycle.append(nextPoint[0])
						current = nextPoint[0]
						findNext = True
						break
			break
		path = path[:i]+newCycle+path[i+1:]
	path = list(map(lambda x: str(x+1), path))[:-1]
	print(1)
	print(" ".join(path))
