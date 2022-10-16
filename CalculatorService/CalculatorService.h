#pragma once

#include "windows.h"
#include <msclr\marshal_cppstd.h>
#include <iostream>
#include <stack>
#include <vector>
#include <string>
#include <cmath>

using namespace System;
//����ֵ���Ÿ�������ż��
enum ABS_ODEVITY {
	ABS_ODD = 1,
	ABS_EVEN = 2,
};

//������������Ȩ�ȼ�
enum PRIO_LV {
	PRIO_LV0 = 0,
	PRIO_LV1 = 1,
	PRIO_LV2 = 2,
	PRIO_LV3 = 3,
	PRIO_LV4 = 4,
};

//��������
class Calculator
{
public:
	Calculator();
	void getFormat();					//���ʽ�Զ����׼��ʽ��
	int getPrior(char c);				//��ȡ�����������ȼ�
	void getPostfix();					//��׺���ʽת��
	void calResult();					//������
	void calculate();					//���㷽��
	double getResult();					//��ȡ���

	std::string operatorSym;					//�������
	std::string infix;						//���ʽ����

private:
	std::vector<std::string> postfix;				//��׺���ʽ����
	std::stack<char> symStack;				//����ջ
	std::stack<double> figStack;				//����ջ
	std::string stdInfix;					//�Զ����׼��ʽ�����ʽ
	double result;						//���ռ�����
};


Calculator::Calculator() {				//���캯������ʼ����Ա����

	//operatorSym = "֧���������+, - , *, /, %���ٷֺţ�, ^���ݴη���, | |������ֵ���������׳ˣ����������ţ�( ) [ ] { }";
	result = 0.0;
}


//���ʽ�Զ����׼��ʽ��
void Calculator::getFormat() {

	stdInfix = infix;

	//ʵ��������
	//for (int i = 0; i < stdInfix.length(); i++) {					//std::string�±���������ʱ���ܻᵼ���������
	for (size_t i = 0; i < stdInfix.size(); i++) {					//std::string.size()����size_type���ͣ������±�����ʱ���������
		if (stdInfix[i] == '-' || stdInfix[i] == '+') {				//-xת��Ϊ0-x��+xת��Ϊ0+x
			if (i == 0) {
				stdInfix.insert(0, 1, '0');
			}
			else if (stdInfix[i - 1] == '(') {
				stdInfix.insert(i, 1, '0');
			}
		}
	}
}

//��ȡ�����������ȼ�
int Calculator::getPrior(char c) {

	if (c == '+' || c == '-') {
		return PRIO_LV1;
	}
	else if (c == '*' || c == '/') {
		return PRIO_LV2;
	}
	else if (c == '%' || c == '^') {
		return PRIO_LV3;
	}
	else if (c == '��') {
		return PRIO_LV4;
	}
	else {
		return PRIO_LV0;
	}
	//else { cout << c << �Ƿ����ţ� << endl; }
}

//��׺���ʽת��
void Calculator::getPostfix() {

	int absNumeber = ABS_ODD;				//����ֵ���Ÿ�������ż��
	std::string tmp;

	//for (int i = 0; i < stdInfix.length(); i++) {
	for (size_t i = 0; i < stdInfix.size(); i++) {					//std::string.size()����size_type���ͣ������±�����ʱ���������
		tmp = "";
		switch (stdInfix[i]) {
		case '+':
		case '-':
		case '*':
		case '/':
		case '%':
		case '^':
		case '!':
			if (symStack.empty() || symStack.top() == '(' || symStack.top() == '[' || symStack.top() == '{' || (symStack.top() == '|' && absNumeber == ABS_ODD)) {
				symStack.push(stdInfix[i]);
			}
			else {
				while (!symStack.empty() && (getPrior(symStack.top()) >= getPrior(stdInfix[i]))) {
					tmp += symStack.top();
					postfix.push_back(tmp);
					symStack.pop();
					tmp = "";
				}
				symStack.push(stdInfix[i]);
			}
			break;
		case '|':
			if (absNumeber == ABS_ODD) {
				symStack.push(stdInfix[i]);
				absNumeber = ABS_EVEN;
			}
			else {
				while (!symStack.empty() && symStack.top() != '|') {
					tmp += symStack.top();
					postfix.push_back(tmp);
					symStack.pop();
					tmp = "";
				}
				if (!symStack.empty() && symStack.top() == '|') {
					tmp += symStack.top();
					postfix.push_back(tmp);						//�����ֵ����'|'�����׺���ʽ�����ھ���ֵ�ļ�����
					symStack.pop();
					absNumeber = ABS_ODD;
				}
			}
			break;
		case '(':
		case '[':
		case '{':
			symStack.push(stdInfix[i]);
			break;
		case ')':
			while (!symStack.empty() && symStack.top() != '(') {
				tmp += symStack.top();
				postfix.push_back(tmp);
				symStack.pop();
				tmp = "";
			}
			if (!symStack.empty() && symStack.top() == '(') {
				symStack.pop();							//�������ų�ջ����
			}
			break;
		case ']':
			while (!symStack.empty() && symStack.top() != '[') {
				tmp += symStack.top();
				postfix.push_back(tmp);
				symStack.pop();
				tmp = "";
			}
			if (!symStack.empty() && symStack.top() == '[') {
				symStack.pop();							//�������ų�ջ����
			}
			break;
		case '}':
			while (!symStack.empty() && symStack.top() != '{') {
				tmp += symStack.top();
				postfix.push_back(tmp);
				symStack.pop();
				tmp = "";
			}
			if (!symStack.empty() && symStack.top() == '{') {
				symStack.pop();							//�������ų�ջ����
			}
			break;
		default:
			if ((stdInfix[i] >= '0' && stdInfix[i] <= '9')) {
				tmp += stdInfix[i];
				while (i + 1 < stdInfix.length() && (stdInfix[i + 1] >= '0' && stdInfix[i + 1] <= '9' || stdInfix[i + 1] == '.')) {		//С������

					tmp += stdInfix[i + 1];			//�����������֣���׷��
					i++;
				}
				if (tmp[tmp.length() - 1] == '.') {
					tmp += '0';						//��x.��x.0����
				}
				postfix.push_back(tmp);
			}
			break;
		}//end switch
	}//end for

	//if(!symStack.empty()) {
	while (!symStack.empty()) {						//��ջ��ʣ����ż����׺���ʽ
		tmp = "";
		tmp += symStack.top();
		postfix.push_back(tmp);
		symStack.pop();
	}
}

//��ȡ������
void Calculator::calResult() {

	std::string tmp;
	double number = 0;
	double op1 = 0, op2 = 0;

	for (int i = 0; i < postfix.size(); i++) {
		tmp = postfix[i];
		if (tmp[0] >= '0' && tmp[0] <= '9') {
			number = atof(tmp.c_str());
			figStack.push(number);
		}
		else if (postfix[i] == "+") {
			if (!figStack.empty()) {
				op2 = figStack.top();
				figStack.pop();
			}
			if (!figStack.empty()) {
				op1 = figStack.top();
				figStack.pop();
			}
			figStack.push(op1 + op2);
		}
		else if (postfix[i] == "-") {
			if (!figStack.empty()) {
				op2 = figStack.top();
				figStack.pop();
			}
			if (!figStack.empty()) {
				op1 = figStack.top();
				figStack.pop();
			}
			figStack.push(op1 - op2);
		}
		else if (postfix[i] == "*") {
			if (!figStack.empty()) {
				op2 = figStack.top();
				figStack.pop();
			}
			if (!figStack.empty()) {
				op1 = figStack.top();
				figStack.pop();
			}
			figStack.push(op1 * op2);
		}
		else if (postfix[i] == "/") {
			if (!figStack.empty()) {
				op2 = figStack.top();
				figStack.pop();
			}
			if (!figStack.empty()) {
				op1 = figStack.top();
				figStack.pop();
			}
			if (op2 != 0) {
				///������Ϊ0��δ������Ĭ��
			}
			figStack.push(op1 / op2);
		}
		else if (postfix[i] == "%") {
			if (!figStack.empty()) {
				op2 = figStack.top();
				figStack.pop();
			}
			if (!figStack.empty()) {
				op1 = figStack.top();
				figStack.pop();
			}
			figStack.push(fmod(op1, op2));			//�ɽ���С������
		}
		else if (postfix[i] == "^") {
			if (!figStack.empty()) {
				op2 = figStack.top();
				figStack.pop();
			}
			if (!figStack.empty()) {
				op1 = figStack.top();
				figStack.pop();
			}
			figStack.push(pow(op1, op2));
		}
		else if (postfix[i] == "|") {
			if (!figStack.empty()) {
				op1 = figStack.top();
				figStack.pop();
			}
			figStack.push(abs(op1));
		}
		else if (postfix[i] == "!") {
			if (!figStack.empty()) {
				op1 = figStack.top();
				figStack.pop();
			}
			if (op1 > 0) {
				//�׳���Ӧ���ڣ�ΪС��ʱ(ת��Ϊ�������)
				double factorial = 1;
				for (int i = 1; i <= op1; ++i)
				{
					factorial *= i;
				}
				op1 = factorial;
			}
			figStack.push(op1);
		}
	}//end for
	if (!figStack.empty()) {
		result = figStack.top();
	}
}

//���㷽��
void Calculator::calculate() {

	getFormat();				//���ʽ�Զ����׼��ʽ��
	getPostfix();				//��׺���ʽת��
	calResult();				//��ȡ�������
}

//��ȡ���
double Calculator::getResult() {
	return result;
}

namespace CalculatorService {
	public ref class Calculatorref
	{
		
		// TODO: Add your methods for this class here.
		public: String^ GetText(String^ input) {
			Calculator cal;
			cal.infix = msclr::interop::marshal_as<std::string>(input);
			cal.calculate();
			return  String::Format(cal.getResult().ToString());
		}

	};
}
