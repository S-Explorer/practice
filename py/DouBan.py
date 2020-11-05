# -*- codeing = utf-8 -*-
# @time :2020/10/29 18:18
# @author:Lwins
# @site:
'''
======================================豆瓣电影TOP250=========================================================
'''

from bs4 import BeautifulSoup                                       #网页解析
import re                                                           #正则表达式
import urllib.request,urllib.error                                  #指定URL，获取网页数据
import xlwt                                                         #进行excel操作
import sqlite3                                                      #进行SQLite数据库操作

def main():
    baseurl = "https://movie.douban.com/top250?start="
    datalist = getData(baseurl)
    # print(datalist)
    # print(len(datalist))
    sava_path = ".\\DATA\\豆瓣TOP250.xls"
    saveData(sava_path,datalist)

#寻找影片的链接
findlink = re.compile(r'<a href="(.*?)">')
#寻找影片的海报图片
findimgsrc = re.compile(r'<img.*src="(.*?)" width="100"/>')#如果后缀re.S可以忽略换行符'''re.S'''
#寻找影片的片名
findtitle = re.compile(r'<span class="title">(.*?)</span>',re.S)
#寻找影片的导演主演
findact = re.compile(r'<p class="">(.*?)<br/>',re.S)
#寻找影片的评分
findgrade = re.compile(r'<span class="rating_num" property="v:average">(.*)</span>')
#寻找影片的评价人数
findnum = re.compile(r'<span>(\d*)人评价</span>')
#寻找影片的金句
findwords = re.compile(r'<span class="inq">(.*)</span>')

#爬取网页的数据
def getData(baseurl):
    datalist = []
    for i in range(0,10):                   #调用循环10次获取全部250部电影的数据
        url = baseurl + str(i*25)
        html = askURL(url)                     #保存获取的网页源码
        # 解析代码
        soup = BeautifulSoup(html,"html.parser")
        Rtitle = ''
        for item in soup.find_all('div',class_='item'):      #查找符合要求的字符串，形成列表
            data = []
            item = str(item)
            # print(item)

            link = re.findall(findlink,item)[0] #获取影片详情的链接
            data.append(link)
            # print(link)

            imgsrc = re.findall(findimgsrc,item)[0] #获取影片的图片的链接
            data.append(imgsrc)
            # print(imgsrc)

            title = re.findall(findtitle,item)
            if len(title) > 1 :
                i = 0
                for name in title:
                    Rtitle = Rtitle + name
                    i += 1
            else:
                Rtitle = title[0]
            data.append(Rtitle)
            # print(Rtitle)
            Rtitle = ''

            act = re.findall(findact,item)[0]
            act = re.sub('\n','',act)
            act = re.sub(' ','',act)
            act = re.sub('/...','',act)
            data.append(act)
            # print(act)

            grade = re.findall(findgrade,item)[0]
            data.append(grade)
            # print(grade)

            num = re.findall(findnum,item)[0]
            data.append(num)
            # print(num)

            words = re.findall(findwords,item)
            if len(words) != 0 :
                words = words[0].replace('。','')
            else:
                pass
            data.append(words)
            # print(words)

            datalist.append(data)
    return datalist


#得到指定的URL的内容
def askURL(url):
    head = {
        "User-Agent": "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/86.0.4240.111 Safari/537.36"
    }
    req = urllib.request.Request(url=url, headers=head)
    html = ""
    try:
        response = urllib.request.urlopen(req)
        html = response.read().decode('utf-8')
        #print(html)
    except urllib.error.URLError as erro:
        if hasattr(erro,"code"):
            print(erro.code)
        if hasattr(erro,"reason"):
            print(erro.reason)
    return html

#保存数据
def saveData(save_path,datalist):
    workbook = xlwt.Workbook(encoding='utf-8')
    worksheet = workbook.add_sheet('豆瓣TOP250')
    worksheet.write(0, 0, '电影详情页')
    worksheet.write(0, 1, '电影海报')
    worksheet.write(0, 2, '电影名')
    worksheet.write(0, 3, '电影内容')
    worksheet.write(0, 4, '电影评分')
    worksheet.write(0, 5, '电影评价人数')
    worksheet.write(0, 6, '电影评语')
    for line in range(1,251):
        for row in range(0,7):
            worksheet.write(line,row,datalist[line-1][row])
    workbook.save(save_path)
    print("保存数据中...")

if __name__ == "__main__":
    #调用函数
    main()