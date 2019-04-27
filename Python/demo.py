#coding:utf-8
import os
from z3 import *
import sys

def chk_bomb(row, col,s, cells):
    #将条件压入栈
    s.push()
    s.add(cells[row][col]==1)
    result = str(s.check())
    #判断后将条件弹出，避免影响后续判断
    s.pop()
    if result == "unsat":
        print ("row=%d col=%d, unsat!"%(row, col))

def start(box_map):
    width = len(box_map[0])
    height = len(box_map)

    s = Solver()
    cells = [[Int('cell_r=%d_c%d'%(r,c)) for c in range(width+2)] for r in range(height+2)]

    for c in range(width+2):
        s.add(cells[0][c]==0)
        s.add(cells[height+1][c]==0)
    for r in range(height+2):
        s.add(cells[r][0]==0)
        s.add(cells[r][width+1]==0)   

    for r in range(1, height+1):
        for c in range(1, width+1):

            t = box_map[r-1][c-1]
            if t in '012345678':
                s.add(cells[r][c]==0)
                s.add(cells[r-1][c-1] + cells[r-1][c] + cells[r-1][c+1] + cells[r][c-1]
                    + cells[r][c+1] + cells[r+1][c-1] + cells[r+1][c] + cells[r+1][c+1]
                    == int(t))

    for r in range(1, height+1):
        for c in range(1, width+1):
            if box_map[r-1][c-1] == '?':
                chk_bomb(r, c, s, cells)

def main():
    box_data = sys.argv[1]
    box_map = box_data.split(',')
    start(box_map)

if __name__ == '__main__':
    main()