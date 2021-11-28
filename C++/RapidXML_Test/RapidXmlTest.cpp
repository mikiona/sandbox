//#include "pch.h"
#include "RapidXmlTest.h"

RapidXmlTest::RapidXmlTest()
{
	//Xml��ǂݍ���DOM
	rapidxml::xml_document<> doc;
	//RapidXml�̃��[�e�B���e�B�N���X�@�ǂݍ���Xml�t�@�C�����I���������ɕێ����Ă����
	rapidxml::file<> file("Test.xml");
	try 
	{
		//Xml�f�[�^���p�[�X����
		doc.parse<0>(file.data());
	}
	catch (rapidxml::parse_error& err) 
	{
		//�G���[���o���猴���Əꏊ(�|�C���^)�������Ă����
		std::cout << err.what() << " " << err.where<char*>();
		return;
	}
	//Xml�t�@�C���̃f�[�^��\��
	show_xml(doc.first_node());
}

void RapidXmlTest::show_xml(rapidxml::xml_node<>* node,int indent)
{
	//�m�[�h���Ȃ���΂Ȃɂ����Ȃ�
	if (node == nullptr) return;
	//���O�����邩
	if (node->name_size() > 0) 
	{
		//�C���f���g���^�u�����
		for (int i = 0; i < indent; i++) std::cout << "\t";
		std::cout << "Node:" << node->name() << "\n"; 
	}
	else return;
	//�l�����邩
	if (node->value_size() > 0)
	{
		//�C���f���g�ƃm�[�h���^�u�����
		for (int i = 0; i < indent + 1; i++) std::cout << "\t";
		std::cout << "Value:" << node->value() << "\n";
	}
	//�q�m�[�h��
	show_xml(node->first_node(),indent + 1);
	//��m�[�h��
	show_xml(node->next_sibling(),indent);
}
