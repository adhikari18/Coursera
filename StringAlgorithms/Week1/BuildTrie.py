#Uses python3
import sys

def build_trie(patterns):
    trie = dict()
    trie[0] = {}
    nodeNumber = 1
    for pattern in patterns:
    	currentNode = 0
    	for c in pattern:
    		if c not in trie[currentNode]:
    			trie[currentNode][c] = nodeNumber
    			trie[nodeNumber] = {}
    			nodeNumber += 1
    		currentNode = trie[currentNode][c]
    return trie


if __name__ == '__main__':
    patterns = sys.stdin.read().split()[1:]
    trie = build_trie(patterns)
    for node in trie:
        for c in trie[node]:
            print("{}->{}:{}".format(node, trie[node][c], c))
