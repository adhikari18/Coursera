//#include "pch.h"
#include <string>
#include <iostream>
#include <set>
#include <vector>
#include <map>
#include <unordered_map>

using namespace std;

typedef vector<int> vi;
typedef vector<vector<int>> vvi;

class DeBruijnGraph 
{
	int _k;
	int _t;

	vector<string> _edges;
	set<string> _edgeSet;

	int _v;
	vector<string> _vertices;
	multimap<string, int> _prefixMap;
	unordered_map<string, int> _vertixMap; // vertex value to index

	// graph
	vvi _adjList;
public:
	// k-mer size and threshold t
	DeBruijnGraph(int k, int t) : _k(k), _t(t), _v(0) {

	}

	void AddRead(const string& r) {
		for (auto i = 0; i < r.size() - _k + 1; i++) 
		{
			auto kmer = r.substr(i, _k);
			if (_edgeSet.find(kmer) == _edgeSet.end()) {
				_edgeSet.insert(kmer);
				_edges.push_back(kmer);
			}
		}
	}

	void BuildDeBruijnGraph() {
		for (auto edge : _edges) {
			auto pre = edge.substr(0, edge.size() - 1);
			auto suf = edge.substr(1);

			_prefixMap.insert({ pre, _v });

			if (_vertixMap.find(pre) == _vertixMap.end()) {
				_vertices.push_back(pre);
				_vertixMap[pre] = _v++;
			}

			if (_vertixMap.find(suf) == _vertixMap.end()) {
				_vertices.push_back(suf);
				_vertixMap[suf] = _v++;
			}
		}

		_adjList.resize(_v);

		for (auto& _edge : _edges)
		{
			auto pre = _edge.substr(0, _edge.size() - 1);
			auto suf = _edge.substr(1);
			auto to_vertex_index = _vertixMap[suf];
			const auto from_vertex_index = _vertixMap[pre];
			_adjList[from_vertex_index].push_back(to_vertex_index);
		}
	}

	int CountBubbles() {
		auto cnt = 0;
		for (auto i = 0; i < _adjList.size(); i++)
			if (_adjList[i].size() >= 2) 
				cnt += CountBubblesFromSource(i, _t);
		return cnt;
	}

private:
	void GetNonOverLappingPaths(vi& path, set<int>& visited, vvi& allPaths, vector<set<int>>& allSets, int l) {
		if (path.size() == l) {
			allPaths.push_back(path);
			allSets.push_back(visited);
			return;
		}

		auto s = path.back();

		if (_adjList[s].empty()) {
			allPaths.push_back(path);
			allSets.push_back(visited);
			return;
		}

		for (int v : _adjList[s])
		{
			if (visited.find(v) == visited.end()) {
				visited.insert(v);
				path.push_back(v);
				GetNonOverLappingPaths(path, visited, allPaths, allSets, l);
				path.pop_back();
				visited.erase(v);
			}			
		}
	}

	string GetPathString(vi& leftPath, vi& rightPath, int merge) {
		string ret;
		for (auto& p : leftPath)
		{
			if (p == merge) {
				ret += to_string(p);
				break;
			}
			ret += to_string(p) + ",";
		}

		ret += ";";

		for (auto& p : rightPath)
		{
			if (p == merge) {
				ret += to_string(p);
				break;
			}
			ret += to_string(p) + ",";
		}

		return ret;
	}


	int CountBubblesFromLeftRightPaths(vector<set<int>>& leftSet, vector<set<int>>& rightSet, vvi& leftPaths, vvi& rightPaths) {
		set<string> mergePaths;

		for (auto i = 0; i < leftSet.size(); i++) {
			auto& s = leftSet[i];
			for (auto& p : rightPaths) {
				auto merged = false;
				for (auto& v : p) {
					if (s.find(v) != s.end()) {
						merged = true;
						auto pathString = GetPathString(leftPaths[i], p, v);
						mergePaths.insert(pathString);
						break; // current path ends
					}
				}

				if (merged) continue;
			}
		}
		return mergePaths.size();
	}

	int CountBubblesFromSource(int s, int t) {
		auto count = 0;
		vi path;
		vvi leftPaths, rightPaths;
		vector<set<int>> leftSet, rightSet;
		set<int> visited;
		const int n_path = _adjList[s].size();

		for (auto i = 0; i < n_path - 1; i++) {
			for (auto j = i + 1; j < n_path; j++) {
				path.clear();
				leftPaths.clear();
				visited.clear();
				leftSet.clear();
				path.push_back(_adjList[s][i]);
				visited.insert(_adjList[s][i]);
				GetNonOverLappingPaths(path, visited, leftPaths, leftSet, t);

				path.clear();
				rightPaths.clear();
				visited.clear();
				rightSet.clear();
				path.push_back(_adjList[s][j]);
				visited.insert(_adjList[s][j]);
				GetNonOverLappingPaths(path, visited, rightPaths, rightSet, t);

				count += CountBubblesFromLeftRightPaths(leftSet, rightSet, leftPaths, rightPaths);
			}
		}

		return count;
	}
};

int main(void) {
	int k, t;
	string read;

	cin >> k >> t;
	DeBruijnGraph graph(k, t);
	/*for (int i = 0; i < 10; i++)
	{
		cin >> read;
		graph.AddRead(read);
	}*/
	while (cin >> read) {
		graph.AddRead(read);
	}

	graph.BuildDeBruijnGraph();

	cout << graph.CountBubbles() << endl;

	return 0;
}
