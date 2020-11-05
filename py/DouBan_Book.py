# -*- codeing = utf-8 -*-
# @time :2020/11/1 17:47
# @author:Lwins
'''
===================================豆瓣书单TOP250============================================================
'''
import re
import xlwt
from bs4 import BeautifulSoup
import urllib.request,urllib.error

def main():
    baseurl = 'https://book.douban.com/top250?start='
    datalist = getdata(baseurl)
    print(len(datalist))
    savadata('.\豆瓣图书TOP250.xls',datalist)
    print('爬取成功!')

findlink = re.compile(r'<a class="nbg" href="(.*?)" onclick.*>')
findimgsrc = re.compile(r'<img src="(.*?)" width.*')
findtitle = re.compile(r'<a href=".*title="(.*)">')
findauthor = re.compile('<p class="pl">(.*?)/.*</p>?')
findgrade = re.compile(r'<span class="rating_nums">(.*)</span>?')
findquote = re.compile(r'<span class="inq">(.*)</span>?')

def getdata(url):
    bookdatalist = []
    for page in range(0,10):
        Rurl = url + str(page*25)
        html = askUrl(Rurl)
        # print(html)
        source = BeautifulSoup(html,"html.parser")
        # print(source)
        for item in source.find_all('tr',class_='item'):
            bookdata = []
            item = str(item)
            # print(item)

            link = re.findall(findlink,item)[0]
            # print(link)
            bookdata.append(link)

            img = re.findall(findimgsrc,item)[0]
            # print(img)
            bookdata.append(img)

            title = re.findall(findtitle,item)[0]
            # print(title)
            bookdata.append(title)

            author = re.findall(findauthor,item)[0]
            # print(author)
            bookdata.append(author)

            grade = re.findall(findgrade,item)[0]
            # print(grade)
            bookdata.append(grade)

            quote = re.findall(findquote,item)
            if len(quote) != 0:
                pass
            else:
                quote = ''
            # print(quote)
            bookdata.append(quote)

            # print(bookdata)
            bookdatalist.append(bookdata)
            # break
    return  bookdatalist

def askUrl(requestURL):
    head = {
        "User-Agent": "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/86.0.4240.111 Safari/537.36"
    }
    req = urllib.request.Request(url=requestURL, headers=head)
    html = ""
    try:
        response = urllib.request.urlopen(req)
        html = response.read().decode('utf-8')
        # print(html)
    except urllib.error.URLError as erro:
        if hasattr(erro, "code"):
            print(erro.code)
        if hasattr(erro, "reason"):
            print(erro.reason)
    return html

def savadata(path,bookdata):
    workbook = xlwt.Workbook(encoding='utf-8')
    worksheet = workbook.add_sheet('豆瓣书单TOP250')
    worksheet.write(0,0,'书籍详情页')
    worksheet.write(0,1,'书籍图片')
    worksheet.write(0,2,'书名')
    worksheet.write(0,3,'作者')
    worksheet.write(0,4,'评分')
    worksheet.write(0,5,'评语')
    for line in range(1,251):
        for row in range(0,6):
            worksheet.write(line,row,bookdata[line - 1][row])
    print('数据导入保存完毕！')
    workbook.save(path)
    print('文件保存完毕！')

if __name__ == '__main__':
    main()

