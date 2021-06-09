#include<iostream>
#include<string>

using namespace std;
//定义结构体
struct contract_man
{
	string name;
	char grander;
	long long number;
};
//增加联系人函数
void add_man(contract_man man[],int len) 
{
	cout << "请输入姓名：" << endl;
	cin >> man[len ].name;
	cout << "请输入性别（0：女性  1：男性）：" << endl;
	cin >> man[len ].grander;
	cout << "请输入号码：" << endl;
	cin >> man[len ].number;
	cout << "输入完毕,请继续" << endl;
}
//显示联系人
void display_man(contract_man man[],int len)
{
	if (len == 0)
	{
		cout << "没有联系人 !" << endl;
	}
	else {
		for (int i = 0; i < len; i++)
		{
			cout << (i + 1) << ".    姓名:" << man[i].name; cout << " \t性别: " << (man[i].grander == 1 ? "男" : "女"); cout << "\t电话: " << man[i].number << endl;
		}
		cout << "打印完毕！" << endl;
	}
}
//删除联系人
void del_man(contract_man man[], int len)
{
	if (len != 0)
	{
		int ch;
		cin >> ch;
		for (int i = ch; i < len; i++)
		{
			man[i - 1].grander = man[i].grander;
			man[i - 1].name = man[i].name;
			man[i - 1].number = man[i].number;
		}
		cout << "删除完毕！" << endl;
	}
	else
	{
		cout << "没有联系人！" << endl;
	}
	

}
//查找联系人
void ser_man(contract_man man[],int len)
{
	if (len != 0)
	{
		string ch;
		cout << "输入你要查找的人的名字：";
		cin >> ch;
		int num = 0;
		for (int i = 0; i < len; i++)
		{
			if (man[i].name == ch)
			{
				cout << i << ".   姓名:" << man[i].name << "\t性别：" << (man[i].grander == 1 ? "男" : "女") << "\t电话：" << man[i].number << endl;
				break;
			}
			num++;
		}
		if (num == len)
		{
			cout << "没有找到要查找的联系人！" << endl;
		}
	}
	else
	{
		cout << "没有联系人" << endl;
	}
	
}
//修改联系人
void edit_man(contract_man man[],int len)
{
	if (len != 0)
	{
		string ch;
		cout << "请输入你想要更改的联系人名字：";
		cin >> ch;
		int num = 0;
		for (int i = 0; i < len; i++)
		{
			if (man[i].name == ch)
			{
				cout << "请输入你要更改的名字：";
				cin >> man[i].name;
				cout << "请输入你要更改的性别:(0：女，1：男)";
				cin >> man[i].grander;
				cout << "请输入你要更改的号码：";
				cin >> man[i].number;
				break;
			}
			num++;
		}
		if (num == len)
		{
			cout << "没有找到要修改的联系人！" << endl;
		}
	}
	else
	{
		cout << "没有联系人!" << endl;
	}
	
}

void main_munu()
{
	cout << "	************************************" << endl;
	cout << "	********* 1.添加联系人 *************" << endl;
	cout << "	********* 2.显示联系人 *************" << endl;
	cout << "	********* 3.删除联系人 *************" << endl;
	cout << "	********* 4.查找联系人 *************" << endl;
	cout << "	********* 5.修改联系人 *************" << endl;
	cout << "	********* 6.清空联系人 *************" << endl;
	cout << "	********* 7.退出通讯录 *************" << endl;
	cout << "	************************************" << endl;

}

//主函数
int main()
{
	int exit = 0;
	contract_man man[1000];
	int num_of_man = 0;
	do
	{
		int num_ch;
		main_munu();
		cin >> num_ch;
		switch (num_ch)
		{
		case 1:
		{
			add_man(man, num_of_man);
			num_of_man++;
			break;

		}
		case 2:
		{
			display_man( man , num_of_man);
			break;
		}
		case 3:
		{
			del_man(man, num_of_man);
			num_of_man--;
			break;
		}
		case 4:
		{
			ser_man(man, num_of_man);
			break;

		}
		case 5:
		{
			edit_man(man, num_of_man);
			break;
		}
		case 6:
		{
			//清空联系人，在逻辑上进行人数清0
			num_of_man = 0;
			break;
		}
		case 7:
		{
			exit = 1;
			cout << "感谢您的使用！" << endl;
		}
		default:
			break;
		}
	} while (exit != 1);
	
	return 0;

}