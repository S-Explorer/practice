# -*- codeing = utf-8 -*-
# @time : 11:22
# @author:12462
# @name:hydraulic.py
# @project_name:learing
#导入模块
import numpy as np
import matplotlib.pyplot as plt
import cmath
import xlwt
#txt数据文件存储
fp = open('.\data.txt','w',encoding = 'utf-8')
fp.write("直动式溢流阀动态系统仿真！\n")
fp.write('T             P(T)               X(T)    \n')
#excel数据文件存储
workbook = xlwt.Workbook(encoding='utf-8')
worksheet = workbook.add_sheet('data')
def main():
     #变量赋值
     d1 = 0.012     #阀芯直径
     r1 = 0.39e+11  #液阻
     r2 = 0.147e+12 #泄露液阻
     i1 = 0.0614    #转动惯量
     c1 = 0.8e-12   #液容
     c2 = 0.2e-4    #弹簧的柔度
     x1 = 0.0014    #阀芯的搭合量
     p0 = 0.6e+6    #出口压力
     pp = 0.3e+7    #压力差Δp
     q1 = 0.46e-3   #泵输入流量
     h1 = 0.0003    #步长
     t = 0.0        #计数
     n1 = 5         #单步迭代次数
     n2 = 55        #总计算步数
     a1 = 1.0915e-04#阀芯面积
     pt = []
     xt = []
     time = []
     #数组赋值
     a = [[-a1*a1*r1/i1, -1/c2, a1 / c1],
          [1 / i1, 0.0, 0.0],
          [-a1 / i1, 0.0, -1 / (r2*c1)]]#状态方程系数
     a = np.array(a,dtype=np.float64)
     b = [(-1, 0), (0, 0), (0, 1)]
     b = np.array(b,dtype=np.float64)
     y = np.zeros((1,3),dtype=np.float64)
     y[0][0] = 0
     y[0][1] = 0
     y[0][2] = p0*c1
     x2 = (q1-p0/r1) / (0.7*3.14*d1*cmath.sqrt(2.0*pp / 900.0))
     u = (258.4402, 0.46e-3)
     u = np.array(u,dtype=np.float64)
     c = -0.7*3.14*d1*cmath.sqrt(2.0 / 900.0)
     k = np.zeros((3,5),dtype = np.float64)
     for i in range(0,3,1):
          k[i][0]=0
     h = [0.0,h1/2.0,h1/2.0,h1]
     h = np.array(h,np.float64)
     p = np.zeros((3,4),dtype = np.float64)
     z = np.zeros((1,3),dtype = np.float64)
     d = np.zeros((3,4),dtype=np.float64)
     #计算程序
     for e in range(1,70,1):
          for g in range(1,6,1):
               t = t + h1
               for j  in range(1,4,1):
                    for i in range(0,3,1):
                         p[i][j] = h[j-1]*k[i][j-1]
                         z[0][i] = y[0][i]
                         z[0][i] = z[0][i] + p[i][j]
                    if (z[0][1]<0):
                         z[0][1] = 0.0
                    if (y[0][1]==0):
                         if(z[0][0]<0):
                              z[0][0] = 0.0
                    if (z[0][2]<0):
                         z[0][2] = 0.0
                    for i in range(0,3,1):
                         d[i][j] = 0.0
                         for l in range(0,3,1):
                              d[i][j] = d[i][j] + a[i][l] * z[0][l]
                         k[i][j] = d[i][j]
                         for s in range(0,2,1):
                              k[i][j] = k[i][j] + b[i][s] * u[s]
                    if (y[0][1] == 0.0 ):
                         if (k[0][j] < 0.0):
                               k[0][j] = 0.0
                    if (y[0][1] == 0.0):
                         if (k [1][j] < 0.0):
                              k[1][j] = 0.0
                    if (y[0][2] == 0.0 ):
                         if ( k[2][j] < 0.0):
                              k[2][j] = 0.0
                    if (y[0][1] > x1):
                         try:
                              k[2][j] = k[2][j] + c * (z[0][1] - x1) * cmath.sqrt(z[0][2] / c1)
                         except Exception as erro :
                              print('No'+str(e)+'line erro')
                         if (y[0][2] == 0.0 ):
                              if( k[2][j] < 0.0):
                                    k[2][j] = 0.0
               for i in range(0,3,1):
                    y[0][i] = y[0][i] + h1 * (k[i][1] + 2 * k[i][2] + 2 * k[i][3] + k[i][4]) / 6.0
               if (y[0][1] < 0.0):
                    y[0][1] = 0.0
               if (y[0][1] == 0.0):
                    if(y[0][0] < 0.0):
                         y[0][0] = 0.0
               if (y[0][2] < 0.0):
                    y[0][2] = 0.0
          fp.write(str(t)+'      '+str(y[0][2]/c1)+'      '+str(y[0][1])+'\n')
          time.append(t)
          pt.append(y[0][2]/c1)
          xt.append(y[0][1])
          worksheet.write(e-1,0,str(t))
          worksheet.write(e-1, 1, str(y[0][2]/c1))
          worksheet.write(e-1, 2, str(y[0][1]))
     #保存数据文件
     workbook.save('.\数据.xls')
     fp.close()
     #绘图
     time = np.array(time)
     pt = np.array(pt)
     xt = np.array(xt)
     plt.figure(1)
     plt.subplot(121)
     plt.plot(time,pt,color = 'r')
     plt.xlabel('时间')
     plt.ylabel('压力')
     plt.title('压力随时间变化曲线')
     plt.legend("压力", loc='upper left')
     plt.subplot(122)
     plt.plot(time,xt,color = 'b')
     plt.xlabel('时间')
     plt.ylabel('位移')
     plt.title('阀芯位移随时间变化曲线')
     plt.legend("位移", loc='upper left')
     plt.show()

if __name__ == '__main__':
    main()