# python3
from decimal import Decimal, getcontext
from copy import deepcopy
getcontext().prec = 6

import math

class Vector(object):
	def __init__(self, coordinates):
		self.coordinates = tuple(coordinates)
		self.dimension = len(coordinates)

	def __eq__(self, v):
		return self.coordinates == v.coordinates

	def __add__(self, other):
		if type(other) == Vector:
			newList = [self.coordinates[i]+other.coordinates[i] for i in range(self.dimension)]
			return Vector(newList)
		if type(other) == int or type(other) == float:
			newList = [self.coordinates[i]+other for i in range(self.dimension)]
			return Vector(newList)

	def __radd__(self, other):
		if type(other) == Vector:
			newList = [self.coordinates[i]+other.coordinates[i] for i in range(self.dimension)]
			return Vector(newList)
		if type(other) == int or type(other) == float:
			newList = [self.coordinates[i]+other for i in range(self.dimension)]
			return Vector(newList)

	def __sub__(self, other):
		if type(other) == Vector:
			newList = [self.coordinates[i]-other.coordinates[i] for i in range(self.dimension)]
			return Vector(newList)
		if type(other) == int or type(other) == float:
			newList = [self.coordinates[i]-other for i in range(self.dimension)]
			return Vector(newList)

	def __rsub__(self, other):
		if type(other) == int or type(other) == float:
			newList = [other - self.coordinates[i] for i in range(self.dimension)]
			return Vector(newList)

	def __mul__(self, other):
		if type(other) == Vector:
			newList = [self.coordinates[i]*other.coordinates[i] for i in range(self.dimension)]
			return sum(newList)
		if type(other) == int or type(other) == float:
			newList = [self.coordinates[i]*other for i in range(self.dimension)]
			return Vector(newList)

	def __rmul__(self, other):
		if type(other) == Vector:
			newList = [self.coordinates[i]*other.coordinates[i] for i in range(self.dimension)]
			return sum(newList)
		if type(other) == int or type(other) == float:
			newList = [self.coordinates[i]*other for i in range(self.dimension)]
			return Vector(newList)

	def __truediv__(self, other):
		if type(other) == Vector:
			newList = [self.coordinates[i]/other.coordinates[i] for i in range(self.dimension)]
			return Vector(newList)
		if type(other) == int or type(other) == float:
			newList = [self.coordinates[i]/other for i in range(self.dimension)]
			return Vector(newList)

	def __pow__(self, other):
		x1,y1,z1 = self.coordinates
		x2,y2,z2 = other.coordinates
		return Vector([y1*z2-y2*z1, z1*x2-z2*x1, x1*y2-x2*y1])

	def __len__(self):
		return len(self.coordinates)

	def __getitem__(self, i):
		return self.coordinates[i]

	def __setitem__(self, i, x):
		self.coordinates[i] = x

	def __deepcopy__(self, memo):
		copiedCoordinates = deepcopy(self.coordinates)
		return Vector(copiedCoordinates)

	def mag(self):
		return sum(n**2 for n in self.coordinates)**0.5

	def unit(self):
		m = self.mag()
		if m == 0:
			return self
		return self / m

	def sim(self, other):	
		return (self*other)/(self.mag()*other.mag())

	def angle(self, other, degree = False):
		a = math.acos(self.sim(other))
		if degree:
			return a * (360/math.pi)
		return a

	def parallel(self, other):
		u1 = self.unit()
		u2 = other.unit()
		if u1==u2 or (u1+u2).mag()==0: return True
		return False

	def orthogonal(self, other, error=0.0001):
		return self*other <= error

	def projectTo(self, other):
		u = other.unit()
		return u * (self * u)

class LinearSystem(object):    
	def __init__(self, equations):
		d = equations[0].dimension
		self.equations = equations
		self.dimension = d


	def swap_rows(self, row1, row2):
		self[row1], self[row2] = self[row2], self[row1]

	def multiply_coefficient_and_row(self, coefficient, row):
		self[row] *= coefficient


	def add_multiple_times_row_to_row(self, coefficient, row_to_add, row_to_be_added_to):
		self[row_to_be_added_to] += coefficient * self[row_to_add]


	def __len__(self):
		return len(self.equations)


	def __getitem__(self, i):
		return self.equations[i]


	def __setitem__(self, i, x):
		self.equations[i] = x

	def __deepcopy__(self, memo):
		copiedEquations = []
		for equation in self.equations:
			copiedEquation = deepcopy(equation)
			copiedEquations.append(copiedEquation)
		return LinearSystem(copiedEquations)

	def triangularForm(self):
		if len(self) < self.dimension: return False
		copied = deepcopy(self)
		for i in range(self.dimension):
			if copied[i][i]==0:
				found = False
				for j in range(i+1, copied.dimension):
					if copied[j][i] != 0:
						found = True
						break
				if not found: return False
				copied.swap_rows(i, j)
			copied[i] /= copied[i][i]
			for j in range(i+1, copied.dimension):
				copied.add_multiple_times_row_to_row(-copied[j][i], i, j)
		return copied

	def solve(self):
		triangle = self.triangularForm()
		if not triangle: return ' '
		for i in range(triangle.dimension):
			if not triangle[i].valid(): return ' '
		for i in range(self.dimension-1, -1, -1):
			for j in range(i):
				triangle.add_multiple_times_row_to_row(-triangle[j][i], i, j)
		result = [0] * triangle.dimension
		for i in range(triangle.dimension):
			result[i] = triangle[i].constant_term
		return result


class Lineq(object):

	NO_NONZERO_ELTS_FOUND_MSG = 'No nonzero elements found'

	def __init__(self, normal_vector, constant_term=0):
		if type(normal_vector) == list or type(normal_vector) == tuple:
			normal_vector = Vector(normal_vector)
		self.dimension = normal_vector.dimension
		self.normal_vector = normal_vector
		self.constant_term = constant_term

		self.set_basepoint()

	def __add__(self, other):
		if type(other) == Lineq:
			new_normal_vector = self.normal_vector + other.normal_vector
			new_constant = self.constant_term + other.constant_term
			return Lineq(new_normal_vector, new_constant)

	def __radd__(self, other):
		if type(other) == Lineq:
			new_normal_vector = self.normal_vector + other.normal_vector
			new_constant = self.constant_term + other.constant_term
			return Lineq(new_normal_vector, new_constant)

	def __sub__(self, other):
		if type(other) == Lineq:
			new_normal_vector = self.normal_vector - other.normal_vector
			new_constant = self.constant_term - other.constant_term
			return Lineq(new_normal_vector, new_constant)

	def __mul__(self, other):
		if type(other) == int or type(other) == float:
			new_normal_vector = self.normal_vector * other
			new_constant = self.constant_term * other
			return Lineq(new_normal_vector, new_constant)

	def __rmul__(self, other):
		if type(other) == int or type(other) == float:
			new_normal_vector = self.normal_vector * other
			new_constant = self.constant_term * other
			return Lineq(new_normal_vector, new_constant)

	def __truediv__(self, other):
		if type(other) == int or type(other) == float:
			new_normal_vector = self.normal_vector / other
			new_constant = self.constant_term / other
			return Lineq(new_normal_vector, new_constant)

	def __len__(self):
		return len(self.normal_vector)

	def __getitem__(self, i):
		return self.normal_vector[i]

	def __setitem__(self, i, x):
		self.normal_vector[i] = x

	def __deepcopy__(self, memo):
		copiedVector = deepcopy(self.normal_vector)
		return Lineq(copiedVector, self.constant_term)

	def valid(self):
		if self.constant_term == 0: return True
		for i in range(len(self)):
			if self[i] != 0: return True
		return False

	def set_basepoint(self):
		try:
			n = self.normal_vector
			c = self.constant_term
			basepoint_coords = [0]*self.dimension

			initial_index = Lineq.first_nonzero_index(n.coordinates)
			initial_coefficient = n.coordinates[initial_index]

			basepoint_coords[initial_index] = c/initial_coefficient
			self.basepoint = Vector(basepoint_coords)

		except Exception as e:
			if str(e) == Lineq.NO_NONZERO_ELTS_FOUND_MSG:
				self.basepoint = None
			else:
				raise e

	def __eq__(self, other):
		if self.dimension != other.dimension:
			return False
		if not self.parallel(other): return False
		s = 0
		for i in range(self.dimension):
			s += other.normal_vector.coordinates[i]*self.basepoint.coordinates[i]
		return s==other.constant_term
	
	def parallel(self, other):
		return self.normal_vector.parallel(other.normal_vector)


	@staticmethod
	def first_nonzero_index(iterable):
		for k, item in enumerate(iterable):
			if not MyDecimal(item).is_near_zero():
				return k
		raise Exception(Lineq.NO_NONZERO_ELTS_FOUND_MSG)

class MyDecimal(Decimal):
	def is_near_zero(self, eps=1e-10):
		return abs(self) < eps

def ReadEquation():
	size = int(input())
	equations = []
	for row in range(size):
		line = list(map(float, input().split()))
		equations.append(Lineq(line[:size],line[size]))
	return LinearSystem(equations)

if __name__ == "__main__":
	s = ReadEquation()
	result = s.solve()
	print(" ".join(list(map(lambda a: format(a, '.6f'), result))))
