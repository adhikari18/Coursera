//#include "pch.h"
#include <iostream>
#include <string>
#include <stdlib.h>
#include <vector>
using namespace std;

static string _str;
int PolyHash(string str, int p, int x, int strLen) {
	int h = 0;
	//int strLen = str.length();
	for (int i = 0; i < strLen; i++) {
		h = (h*x + str.at(i)) % p;
	}
	return h;

};

int PolyHash(int p, int x, int strLen, int startInd) {
	int h = 0;
	//int strLen = str.length();
	for (int i = startInd; i < startInd + strLen; i++) {
		h = (h*x + _str.at(i)) % p;
	}
	return h;
}

bool AreEqual(string S1, string S2) {
	for (int i = 0; i < S1.length(); i++) {
		if (S1[i] != S2[i]) {
			return false;
		}
	}
	return true;
}


void RabinKarp(string T, string P) {
	int prime = 1000000007;
	int x = rand() % (prime - 1) + 1;
	int tLen = T.length();
	int pLen = P.length();
	vector<int> result;
	int pHash = PolyHash(P, prime, x, pLen);
	int tHash;
	for (int i = 0; i <= tLen - pLen; i++) {
		//tHash = PolyHash(tempStr, prime, x, pLen);
		tHash = PolyHash(prime, x, pLen, i);
		if (tHash != pHash) {
			continue;
		}
		string tempStr = T.substr(i, pLen);
		if (AreEqual(tempStr, P)) {
			result.push_back(i);
		}
	}

	for (int j = 0; j < result.size(); j++) {
		cout << result[j] << " ";
	}
	cout << "\n";
};

int main(int argc, char* argv[]) {

	string pattern, text;

	getline(cin, pattern);
	getline(cin, text);
	_str = text;

	RabinKarp(text, pattern);
	return 0;
}




