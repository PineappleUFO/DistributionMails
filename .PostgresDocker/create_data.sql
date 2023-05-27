
--Создание БД
CREATE DATABASE mailer
    WITH
    ENCODING = 'UTF8'
    LC_COLLATE = 'ru_RU.UTF-8'
    LC_CTYPE = 'ru_RU.UTF-8'
    TABLESPACE = pg_default
    CONNECTION LIMIT = -1
    TEMPLATE = template0;
	

--SET search_path TO mailer;
--создание таблиц

--status
create table if not exists public.status
(
    status_id    serial  not null
        constraint status_pk
            primary key,
    status_name  varchar not null,
    status_color varchar
);

comment on table public.status is 'Статус в ветке в дереве распределения';


--resolution
create table if not exists public.fast_resolution
(
    resolution_id   serial  not null
        constraint fast_resolution_pk
            primary key,
    resolution_text varchar not null
);

comment on table public.fast_resolution is 'Коллекция быстрых резолюций';


--sender
create table if not exists public.sender
(
    sender_id   serial  not null
        constraint sender_pk
            primary key,
    sender_name varchar not null
);

comment on table public.sender is 'коллекция отправителей';


--projects
create table if not exists public.projects
(
    project_id  serial
        constraint projects_pk
            primary key,
    sender_name varchar not null
);

comment on table public.projects is 'коллекция проектов';

alter table public.projects add project_color varchar;

--deps
create table if not exists public.deps
(
    dep_id    serial  not null
        constraint deps_pk
            primary key,
    dep_name  varchar not null,
    dep_about varchar not null
);

comment on table public.deps is 'коллекция отделов';


--positions
create table if not exists public.positions
(
    position_id   serial  not null
        constraint positions_pk
            primary key,
    position_name varchar not null
);

comment on table public.positions is 'Должности';


--users
create table public.users
(
    user_id     serial  not null
        constraint users_pk
            primary key,
    family      varchar not null,
    name        varchar not null,
    surname     varchar,
	login		varchar,
    photo       bytea,
    phone       varchar,
    id_dep      integer not null
        constraint users_deps_dep_id_fk
            references deps,
    id_position integer not null
        constraint users_positions_position_id_fk
            references positions
);

comment on table public.users is 'коллекция пользователей';


--distribution_counter
create table public.distribution_counter
(
    id_user             integer not null
        constraint distribution_counter___fk
            references users,
    distributed_user_id integer not null
        constraint distribution_counter_users_user_id_fk
            references users,
    count               integer not null
);

comment on table public.distribution_counter is 'таблица количества распределений на человека';

comment on column public.distribution_counter.id_user is 'кто распределил';

comment on column public.distribution_counter.distributed_user_id is 'кому распределил';

comment on column public.distribution_counter.count is 'сколько раз';

--users_replacement
create table if not exists public.users_replacement
(
    who_user_id  integer not null
        constraint users_replacement_users_user_id_fk
            references users,
    whom_user_id integer not null
        constraint users_replacement_users_user_id_fk2
            references users,
    term         date    not null
);

comment on table public.users_replacement is 'Таблица заместителей';

comment on column public.users_replacement.who_user_id is 'кто замещает';

comment on column public.users_replacement.whom_user_id is 'кого замещает';

comment on column public.users_replacement.term is 'срок окончания замещения';



--outgoing_mail
create table if not exists public.outgoing_mail
(
    mail_id     serial    not null
        constraint outgoing_mail_pk
            primary key,
    number      varchar   not null,
    date_export timestamp not null,
    date_answer timestamp,
    theme       varchar   not null,
    text        varchar
);

comment on table public.outgoing_mail is 'таблица исходящих писем';

comment on column public.outgoing_mail.number is 'номер исходящего письма';

comment on column public.outgoing_mail.date_export is 'дата отправки';

comment on column public.outgoing_mail.date_answer is 'дата написания ответа';


--incoming_mail
create table if not exists public.incoming_mail
(
    mail_id          serial    not null
        constraint incoming_mail_pk
            primary key,
    number           varchar   not null,
    date_input       timestamp not null,
    date_answer      date,
    theme            varchar   not null,
    responsible      integer
        constraint incoming_mail_users_user_id_fk
            references users,
    id_project       integer
        constraint incoming_mail_projects_project_id_fk
            references projects,
    id_sender        integer   not null
        constraint incoming_mail_sender_sender_id_fk
            references sender,
    id_outgoing_mail integer
        constraint incoming_mail_outgoing_mail_mail_id_fk
            references outgoing_mail
);

comment on table public.incoming_mail is 'таблица вхоядщих писем';

comment on column public.incoming_mail.number is 'номер входящего письма';

comment on column public.incoming_mail.responsible is 'главный ответсвенный за письмо';

comment on column public.incoming_mail.id_project is 'проект';

comment on column public.incoming_mail.id_sender is 'отправитель';

comment on column public.incoming_mail.id_outgoing_mail is 'ссылка на ответ (исходящее письмо)';


--distribution_tree
create table if not exists public.distribution_tree
(
    id             serial    not null
        constraint distribution_tree_pk
            primary key,
    id_mail        integer   not null
        constraint distribution_tree_incoming_mail_mail_id_fk
            references incoming_mail,
    id_user        integer   not null
        constraint distribution_tree_users_user_id_fk
            references users,
    id_status      integer
        constraint distribution_tree_status_status_id_fk
            references status,
    up_id          integer,
    deadline       timestamp not null,
    resolution     varchar,
    is_responsible boolean,
    is_replying    boolean,
    date_add       timestamp default CURRENT_TIMESTAMP,
    log            varchar
);

comment on table public.distribution_tree is 'дерево распределения';

comment on column public.distribution_tree.up_id is 'ссылка на верхний уровень';

comment on column public.distribution_tree.resolution is 'личная резолюция';

comment on column public.distribution_tree.is_responsible is 'это ответсвенный?';

comment on column public.distribution_tree.is_replying is 'это отвечающий?';

comment on column public.distribution_tree.date_add is 'дата добавления';


--users_favorite_mail
create table if not exists public.users_favorite_mail
(
    id_mail integer not null
        constraint users_favorite_mail_incoming_mail_mail_id_fk
            references incoming_mail,
    id_user integer not null
        constraint users_favorite_mail_users_user_id_fk
            references users
);

comment on table public.users_favorite_mail is 'Избранные письма пользователей';

--distributed_to_user
create table if not exists public.distributed_to_user
(
    id_mail integer not null
        constraint distributed_to_user_incoming_mail_mail_id_fk
            references incoming_mail,
    id_user integer not null
        constraint distributed_to_user_users_user_id_fk
            references users
);

comment on table public.distributed_to_user is 'Распределенные пользователю письма';

--users_archive_mails
create table if not exists public.users_archive_mails
(
    id_mail integer not null
        constraint users_archive_mails_incoming_mail_mail_id_fk
            references incoming_mail,
    id_user integer not null
        constraint users_archive_mails_users_user_id_fk
            references users
);

comment on table public.users_archive_mails is 'архив распределенных писем пользователя';



-- Создание ролей
CREATE  ROLE ogk;
CREATE ROLE automationdepartment;
CREATE ROLE documentationdepartment;
CREATE ROLE testingdepartment;
CREATE ROLE qualityandreliabilitydepartment;
CREATE ROLE constructiondepartment;
CREATE ROLE materialsdepartment;
CREATE ROLE mechanicsdepartment;
CREATE ROLE electronicsdepartment;
CREATE ROLE legaldpartment;
CREATE ROLE management;

-- Создание пользователей и назначение ролей
CREATE USER petrovaom PASSWORD 'petrovaom';
GRANT ogk TO petrovaom;
GRANT ALL PRIVILEGES ON DATABASE mailer TO petrovaom;
GRANT SELECT, INSERT, UPDATE, DELETE ON ALL TABLES IN SCHEMA public TO petrovaom;

CREATE USER medvedevii PASSWORD 'medvedevii';
GRANT ogk TO medvedevii;
GRANT ALL PRIVILEGES ON DATABASE mailer TO medvedevii;
GRANT SELECT, INSERT, UPDATE, DELETE ON ALL TABLES IN SCHEMA public TO medvedevii;

CREATE USER ivanovaa PASSWORD 'ivanovaa';
GRANT ogk TO ivanovaa;
GRANT ALL PRIVILEGES ON DATABASE mailer TO ivanovaa;
GRANT SELECT, INSERT, UPDATE, DELETE ON ALL TABLES IN SCHEMA public TO ivanovaa;

CREATE USER smirnovaom PASSWORD 'smirnovaom';
GRANT ogk TO smirnovaom;
GRANT ALL PRIVILEGES ON DATABASE mailer TO smirnovaom;
GRANT SELECT, INSERT, UPDATE, DELETE ON ALL TABLES IN SCHEMA public TO smirnovaom;

CREATE USER nikitinaa PASSWORD 'nikitinaa';
GRANT ogk TO nikitinaa;
GRANT ALL PRIVILEGES ON DATABASE mailer TO nikitinaa;
GRANT SELECT, INSERT, UPDATE, DELETE ON ALL TABLES IN SCHEMA public TO nikitinaa;

CREATE USER zaytsevaem PASSWORD 'zaytsevaem';
GRANT ogk TO zaytsevaem;
GRANT ALL PRIVILEGES ON DATABASE mailer TO zaytsevaem;
GRANT SELECT, INSERT, UPDATE, DELETE ON ALL TABLES IN SCHEMA public TO zaytsevaem;

CREATE USER pavlovda PASSWORD 'pavlovda';
GRANT automationdepartment TO pavlovda;
GRANT ALL PRIVILEGES ON DATABASE mailer TO pavlovda;
GRANT SELECT, INSERT, UPDATE, DELETE ON ALL TABLES IN SCHEMA public TO pavlovda;

CREATE USER kovalevaaa PASSWORD 'kovalevaaa';
GRANT automationdepartment TO kovalevaaa;
GRANT ALL PRIVILEGES ON DATABASE mailer TO kovalevaaa;
GRANT SELECT, INSERT, UPDATE, DELETE ON ALL TABLES IN SCHEMA public TO kovalevaaa;

CREATE USER sokolovaea PASSWORD 'sokolovaea';
GRANT automationdepartment TO sokolovaea;
GRANT ALL PRIVILEGES ON DATABASE mailer TO sokolovaea;
GRANT SELECT, INSERT, UPDATE, DELETE ON ALL TABLES IN SCHEMA public TO sokolovaea;

CREATE USER grigoryevas PASSWORD 'grigoryevas';
GRANT automationdepartment TO grigoryevas;
GRANT ALL PRIVILEGES ON DATABASE mailer TO grigoryevas;
GRANT SELECT, INSERT, UPDATE, DELETE ON ALL TABLES IN SCHEMA public TO grigoryevas;

CREATE USER nikolayevaam PASSWORD 'nikolayevaam';
GRANT documentationdepartment TO nikolayevaam;
GRANT ALL PRIVILEGES ON DATABASE mailer TO nikolayevaam;
GRANT SELECT, INSERT, UPDATE, DELETE ON ALL TABLES IN SCHEMA public TO nikolayevaam;

CREATE USER petrovsi PASSWORD 'petrovsi';
GRANT documentationdepartment TO medvedevii;
GRANT ALL PRIVILEGES ON DATABASE mailer TO petrovsi;
GRANT SELECT, INSERT, UPDATE, DELETE ON ALL TABLES IN SCHEMA public TO petrovsi;

CREATE USER zakharovdb PASSWORD 'zakharovdb';
GRANT legaldpartment TO zakharovdb;
GRANT ALL PRIVILEGES ON DATABASE mailer TO zakharovdb;
GRANT SELECT, INSERT, UPDATE, DELETE ON ALL TABLES IN SCHEMA public TO zakharovdb;


--заполнение данных
insert into public.status (status_id, status_name, status_color)
values  (1, 'Выполнено', '06d6a0'),
        (2, 'Принято', 'ffd166'),
        (3, 'Отклонено', 'ef476f'),
        (4, 'Прочитано', '118ab2');
COMMIT;

insert into public.fast_resolution (resolution_id, resolution_text)
values  (1, 'В работу'),
        (2, 'В рабочем порядке'),
        (4, 'Для расмотрения'),
        (3, 'К сведению'),
        (5, 'Согласовать');

COMMIT;
INSERT INTO public.sender (sender_name) VALUES
('ООО Рога и копыта'),
('ЗАО Мир технологий'),
('АО Прогрессивные решения'),
('ООО Инновационные сервисы'),
('ПАО Строительная группа'),
('НИИ Новые технологии'),
('ООО Экспертные решения'),
('АО Медиафокус'),
('ООО Технодинамика'),
('ПАО Южная энергетика');
COMMIT;

insert into public.projects (project_id, sender_name, project_color)
values  (1, '101-Алмаз', 'B9F2FF'),
        (2, '105-Рубин', 'c7004c'),
        (3, '107-Изумруд', '50c878'),
        (4, '108-Топаз', '13bbaf');
COMMIT;

insert into public.deps (dep_id, dep_name, dep_about)
values  (1, 'Отдел проектирования', 'Отвечает за разработку технических проектов и чертежей'),
        (2, 'Отдел конструкций', 'Отвечает за создание и анализ конструкций продуктов'),
        (3, 'Отдел электроники', 'Отвечает за проектирование электронных компонентов и схем'),
        (4, 'Отдел механики', 'Отвечает за проектирование и анализ механических систем и деталей'),
        (5, 'Отдел материалов', 'Отвечает за выбор и анализ материалов для конструкций'),
        (6, 'Отдел испытаний', 'Отвечает за проведение испытаний и проверку работоспособности продуктов'),
        (7, 'Отдел автоматизации', 'Отвечает за разработку автоматизированных систем и устройств'),
        (8, 'Отдел качества и надежности', 'Отвечает за контроль качества и надежности продуктов'),
        (9, 'Отдел документации', 'Отвечает за подготовку технической документации и спецификаций'),
        (10, 'Отдел производства', 'Отвечает за организацию и управление производственными процессами'),
        (11, 'ОГК', 'Отдел главных конструкторов'),
        (12, 'Руководство', 'Руководящий состав'),
		(13, 'Отдел разработки', 'Отвечает за разработку ПО'),
		(14, 'Отдел трубопровода', 'Отвечает за труб и трубопроводов');
		
COMMIT;

insert into public.positions (position_id, position_name)
values  (1, 'Главный инженер'),
        (2, 'Старший инженер'),
        (3, 'Инженер-конструктор'),
        (4, 'Электронный инженер'),
        (5, 'Механик-конструктор'),
        (6, 'Инженер-материаловед'),
        (7, 'Инженер по испытаниям'),
        (8, 'Специалист по автоматизации'),
        (9, 'Инженер по качеству'),
        (10, 'Специалист по документации'),
        (11, 'Программист'),
        (12, 'Начальник отдела'),
        (13, 'Начальник сектора'),
        (14, 'Программный инженер'),
        (15, 'Главный конструктор'),
        (16, 'Помошник главного конструктора'),
        (17, 'Генеральный директор'),
        (18, 'Заместитель генерального директора по персоналу'),
        (19, 'Заместитель генерального директора по финансам');
COMMIT;


insert into public.users (user_id, family, name, surname, photo, phone, id_dep, id_position, login)
values  (92, 'Иванов', 'Иван', 'Иванович', null, '101', 1, 12, 'ivanovii'),
        (157, 'Захаров', 'Дмитрий', 'Борисович', null, '5001', 12, 17, 'zakharovdb'),
        (97, 'Богданов', 'Даниил', 'Сергеевич', null, '106', 1, 3, 'bogdanovds'),
        (123, 'Медведева', 'Мария', 'Ивановна', null, '602', 6, 13, 'medvedevami'),
        (100, 'Морозов', 'Андрей', 'Владимирович', null, '203', 2, 3, 'morozovav'),
        (114, 'Комаров', 'Анатолий', 'Александрович', null, '405', 4, 5, 'komarovaa'),
        (125, 'Зайцева', 'Ольга', 'Дмитриевна', null, '604', 6, 7, 'zaytsevaod'),
        (99, 'Васильева', 'Екатерина', 'Сергеевна', null, '202', 2, 13, 'vasilevaes'),
        (96, 'Кузьмина', 'Мария', 'Александровна', null, '105', 1, 3, 'kuzminama'),
        (134, 'Медведев', 'Артем', 'Дмитриевич', null, '803', 8, 9, 'medvedevad'),
        (137, 'Николаева', 'Анна', 'Максимовна', null, '902', 9, 13, 'nikolayevaam'),
        (115, 'Голубев', 'Сергей', 'Иванович', null, '406', 4, 5, 'golubevsi'),
        (159, 'Новикова', 'Елена', 'Максимовна', null, '4002', 12, 18, 'novikovaem'),
        (142, 'Никитин', 'Сергей', 'Викторович', null, '1003', 10, 3, 'nikitinsv'),
        (133, 'Сорокина', 'Анастасия', 'Алексеевна', null, '802', 8, 13, 'sorokinaaa'),
        (110, 'Лебедев', 'Илья', 'Анатольевич', null, '401', 4, 12, 'levedevia'),
        (107, 'Кузнецова', 'Мария', 'Александровна', null, '304', 3, 3, 'kuznetsovama'),
        (103, 'Сорокин', 'Илья', 'Сергеевич', null, '206', 2, 3, 'sorokinis'),
        (93, 'Петров', 'Петр', 'Петрович', null, '102', 1, 13, 'petrovpp'),
        (95, 'Смирнова', 'Анна', 'Ивановна', null, '104', 1, 3, 'smirnovaan'),
        (154, 'Петрова', 'Ольга', 'Максимовна', null, '4004', 11, 16, 'petrovaom'),
        (119, 'Морозова', 'Анастасия', 'Игоревна', null, '504', 5, 6, 'morozovaai'),
        (109, 'Волков', 'Андрей', 'Владимирович', null, '306', 3, 3, 'volkovav'),
        (124, 'Васильев', 'Илья', 'Андреевич', null, '603', 6, 7, 'vasilevia'),
        (111, 'Ковалев', 'Александр', 'Игоревич', null, '+402', 4, 13, 'kovalevai'),
        (138, 'Петров', 'Сергей', 'Игоревич', null, '903', 9, 10, 'petrovsi'),
        (153, 'Медведев', 'Игорь', 'Игоревич', null, '4003', 11, 15, 'medvedevii'),
        (149, 'Новикова', 'Ольга', 'Максимовна', null, '3004', 14, 11, 'novikovaom'),
        (94, 'Сидоров', 'Алексей', 'Николаевич', null, '103', 1, 3, 'sidorovan'),
        (126, 'Николаев', 'Максим', 'Алексеевич', null, '605', 6, 7, 'nikolayevma'),
        (135, 'Комарова', 'Екатерина', 'Александровна', null, '804', 8, 9, 'komarovaea'),
        (121, 'Смирнов', 'Андрей', 'Иванович', null, '506', 5, 6, 'smirnovai'),
        (118, 'Соловьев', 'Дмитрий', 'Викторович', null, '503', 5, 6, 'solovevdv'),
        (127, 'Иванова', 'Екатерина', 'Ивановна', null, '606', 6, 7, 'ivanovaei'),
        (98, 'Козлов', 'Дмитрий', 'Павлович', null, '201', 2, 12, 'kozlovdp'),
        (136, 'Исаев', 'Михаил', 'Владимирович', null, '901', 9, 12, 'isayevmv'),
        (122, 'Сидоров', 'Николай', 'Сергеевич', null, '601', 6, 12, 'sidorovns'),
        (158, 'Петров', 'Дмитрий', 'Игоревич', null, '5001', 12, 1, 'petrovdi'),
        (151, 'Иванов', 'Андрей', 'Александровна', null, '4001', 11, 15, 'ivanovaa'),
        (117, 'Комаров', 'Арсений', 'Антонович', null, '502', 5, 13, 'komarovaa'),
        (160, 'Соколов', 'Игорь', 'Сергеевич', null, '4003', 12, 19, 'sokolovis'),
        (130, 'Павлов', 'Дмитрий', 'Алексеевич', null, '703', 7, 8, 'pavlovda'),
        (112, 'Зайцев', 'Денис', 'Алексеевич', null, '403', 4, 5, 'zaytsevda'),
        (140, 'Зайцев', 'Анатолий', 'Игоревич', null, '1001', 10, 12, 'zaytsevai'),
        (144, 'Соколов', 'Александр', 'Сергеевич', null, '2001', 13, 12, 'sokolovas'),
        (131, 'Ковалева', 'Анна', 'Анатольевна', null, '704', 7, 8, 'kovalevaaa'),
        (113, 'Никитина', 'Татьяна', 'Сергеевна', null, '404', 4, 5, 'nikitinats'),
        (108, 'Медведев', 'Алексей', 'Алексеевич', null, '305', 3, 3, 'medvedevaa'),
        (116, 'Тихонов', 'Игорь', 'Владиславович', null, '501', 5, 12, 'tihonoviv'),
        (129, 'Соколова', 'Елена', 'Александровна', null, '702', 7, 13, 'sokolovaea'),
        (148, 'Кузнецов', 'Михаил', 'Игоревич', null, '3003', 14, 3, 'kuznetsovmi'),
        (105, 'Павлова', 'Ольга', 'Викторовна', null, '302', 3, 13, 'pavlovaov'),
        (101, 'Новиков', 'Максим', 'Дмитриевич', null, '204', 2, 3, 'novikovmd'),
        (146, 'Иванов', 'Максим', 'Сергеевич', null, '3001', 14, 12, 'ivanovms'),
        (141, 'Козлова', 'Екатерина', 'Владимировна', null, '1002', 10, 13, 'kozlovaev'),
        (152, 'Смирнова', 'Ольга', 'Максимовна', null, '4002', 11, 15, 'smirnovaom'),
        (143, 'Кузьмин', 'Игорь', 'Игоревич', null, '1004', 10, 3, 'kuzminii'),
        (128, 'Григорьев', 'Артем', 'Сергеевич', null, '701', 7, 12, 'grigoryevas'),
        (139, 'Смирнова', 'Елена', 'Александровна', null, '904', 9, 10, 'smirnovaea'),
        (104, 'Федоров', 'Сергей', 'Андреевич', null, '301', 3, 12, 'fedorovsa'),
        (106, 'Соколов', 'Никита', 'Петрович', null, '303', 3, 3, 'sokolovnp'),
        (147, 'Смирнова', 'Анастасия', 'Александровна', null, '3002', 14, 13, 'smirnovaaa'),
        (132, 'Кузнецов', 'Максим', 'Иванович', null, '801', 8, 12, 'kuznetsovmi'),
        (145, 'Комаров', 'Михаил', 'Алексеевич', null, '2002', 13, 3, 'komarovma'),
        (150, 'Соколов', 'Алексей', 'Сергеевич', null, '3005', 14, 3, 'sokolovas'),
        (155, 'Никитин', 'Алексей', 'Алексеевич', null, '4005', 11, 16, 'nikitinaa'),
        (156, 'Зайцева', 'Елена', 'Максимовна', null, '4006', 11, 16, 'zaytsevaem'),
        (120, 'Петров', 'Алексей', 'Максимович', null, '505', 5, 6, 'petrovam'),
        (102, 'Исаева', 'Ольга', 'Александровна', null, '205', 2, 3, 'isaevaoa');
COMMIT;

insert into public.incoming_mail (mail_id, number, date_input, date_answer, theme, responsible, id_project, id_sender, id_outgoing_mail)
values  (1, '1001Э', '2023-02-15 09:30:00.000000', null, 'По вопросу модификации конструкции', null, 2, 5, null),
        (2, '1002Э', '2023-03-05 14:45:00.000000', null, 'Запрос на техническую спецификацию', null, 3, 8, null),
        (3, '1003Э', '2023-04-10 11:00:00.000000', null, 'Проект согласования технической документации', null, 1, 3, null),
        (4, '1004Э', '2023-01-20 16:15:00.000000', null, 'О необходимости проведения испытаний', null, 4, 9, null),
        (5, '1005Э', '2023-03-28 10:00:00.000000', null, 'Согласование изменений в чертежах', null, 2, 7, null),
        (6, '1006Э', '2023-04-05 13:30:00.000000', null, 'Запрос на уточнение технических требований', null, 3, 4, null),
        (7, '1007Э', '2023-02-18 15:45:00.000000', null, 'Согласование структуры изделия', null, 1, 6, null),
        (8, '1008Э', '2023-05-10 12:00:00.000000', null, 'Запрос на изменение материалов', null, 4, 1, null),
        (9, '1009Э', '2023-01-10 09:00:00.000000', null, 'По вопросу улучшения электрической схемы', null, 2, 2, null),
        (10, '1010Э', '2023-03-17 11:30:00.000000', null, 'Согласование технического задания', null, 3, 9, null),
        (11, '1011Э', '2023-04-25 14:00:00.000000', null, 'Запрос на разработку 3D-модели', null, 1, 4, null),
        (12, '1012Э', '2023-02-08 10:15:00.000000', null, 'О предоставлении технической документации', null, 4, 7, null),
        (13, '1013Э', '2023-05-05 13:45:00.000000', null, 'Согласование дополнительных спецификаций', null, 2, 3, null),
        (14, '1014Э', '2023-01-15 16:30:00.000000', null, 'Запрос на расчет прочности конструкции', null, 3, 10, null),
        (15, '1015Э', '2023-03-22 09:30:00.000000', null, 'По вопросу оптимизации производственных процессов', null, 1, 6, null),
        (16, '1016Э', '2023-04-12 12:45:00.000000', null, 'О необходимости доработки узла', null, 4, 8, null),
        (17, '1017Э', '2023-02-25 15:00:00.000000', null, 'Запрос на обновление проектной документации', null, 2, 5, null),
        (18, '1018Э', '2023-05-15 10:30:00.000000', null, 'Согласование изменений в спецификации материалов', null, 3, 2, null),
        (19, '1019Э', '2023-01-05 13:00:00.000000', null, 'О предоставлении расчетной документации', null, 1, 9, null),
        (20, '1020Э', '2023-03-30 16:15:00.000000', null, 'Запрос на разработку нового модуля', null, 4, 4, null),
        (21, '1021Э', '2023-02-13 10:00:00.000000', null, 'По вопросу применения нового материала', null, 2, 7, null),
        (22, '1022Э', '2023-04-08 13:30:00.000000', null, 'О необходимости проведения проверки прототипа', null, 3, 1, null),
        (23, '1023Э', '2023-01-22 15:45:00.000000', null, 'Запрос на уточнение размеров детали', null, 1, 3, null),
        (24, '1024Э', '2023-05-12 12:00:00.000000', null, 'Согласование эргономических параметров', null, 4, 6, null),
        (25, '1025Э', '2023-01-12 09:00:00.000000', null, 'По вопросу улучшения электрической цепи', null, 2, 9, null),
        (26, '1026Э', '2023-03-20 11:30:00.000000', null, 'Запрос на разработку технической спецификации', null, 3, 2, null),
        (27, '1027Э', '2023-04-28 14:00:00.000000', null, 'О проведении совещания по обновлению стандартов', null, 1, 5, null),
        (28, '1028Э', '2023-02-10 10:15:00.000000', null, 'Запрос на предоставление технической документации', null, 4, 8, null),
        (29, '1029Э', '2023-05-08 13:45:00.000000', null, 'Согласование изменений в комплектации', null, 2, 4, null),
        (30, '1030Э', '2023-01-18 16:30:00.000000', null, 'Запрос на расчет прочности сварных соединений', null, 3, 7, null),
        (31, '1031Э', '2023-03-25 09:30:00.000000', null, 'По вопросу оптимизации производственных ресурсов', null, 1, 10, null),
        (32, '1032Э', '2023-04-17 12:45:00.000000', null, 'О необходимости доработки программного обеспечения', null, 4, 3, null),
        (33, '1033Э', '2023-02-28 15:00:00.000000', null, 'Запрос на обновление технической документации', null, 2, 6, null),
        (34, '1034Э', '2023-05-17 10:30:00.000000', null, 'Согласование изменений в проекте конструкции', null, 3, 1, null),
        (35, '1035Э', '2023-01-08 13:00:00.000000', null, 'О предоставлении технической спецификации', null, 1, 9, null),
        (36, '1036Э', '2023-03-22 16:15:00.000000', null, 'Запрос на разработку детального чертежа', null, 4, 4, null),
        (37, '1037Э', '2023-02-05 10:00:00.000000', null, 'По вопросу применения новой технологии', null, 2, 7, null),
        (38, '1038Э', '2023-04-03 13:30:00.000000', null, 'О необходимости проведения испытаний нагрузки', null, 3, 2, null),
        (39, '1039Э', '2023-01-25 15:45:00.000000', null, 'Запрос на уточнение параметров покрытия', null, 1, 3, null),
        (40, '1040Э', '2023-05-10 12:00:00.000000', null, 'Согласование дизайна внешнего вида продукта', null, 4, 6, null),
        (41, '1041Э', '2023-01-15 09:00:00.000000', null, 'По вопросу модификации электронной схемы', null, 2, 9, null),
        (42, '1042Э', '2023-03-27 11:30:00.000000', null, 'Запрос на разработку структурной схемы', null, 3, 1, null),
        (43, '1043Э', '2023-04-22 14:00:00.000000', null, 'О проведении совещания по выбору поставщика', null, 1, 5, null),
        (44, '1044Э', '2023-02-13 10:15:00.000000', null, 'Запрос на предоставление технических расчетов', null, 4, 8, null),
        (45, '1045Э', '2023-05-05 13:45:00.000000', null, 'Согласование изменений в компоновке изделия', null, 2, 4, null),
        (46, '1046Э', '2023-01-20 16:30:00.000000', null, 'Запрос на проведение анализа прочности', null, 3, 7, null),
        (47, '1047Э', '2023-03-18 09:30:00.000000', null, 'По вопросу оптимизации энергопотребления', null, 1, 10, null),
        (48, '1048Э', '2023-04-10 12:45:00.000000', null, 'О необходимости доработки инженерных решений', null, 4, 3, null),
        (49, '1049Э', '2023-02-23 15:00:00.000000', null, 'Запрос на обновление проектной документации', null, 2, 6, null),
        (50, '1050Э', '2023-05-15 10:30:00.000000', null, 'Согласование изменений в комплектующих изделий', null, 3, 1, null);
COMMIT;

insert into public.outgoing_mail (mail_id, number, date_export, date_answer, theme, text)
values  (1, '101Э', '2023-04-01 14:30:00.000000', '2023-04-01 14:30:00.000000', 'Ответ на запрос о разработке РКД', 'Ваш запрос о разработке рабочей конструкторской документации получен. Мы приступим к разработке РКД и в ближайшее время предоставим вам результаты.'),
        (2, '102Э', '2023-02-28 09:45:00.000000', '2023-02-28 09:45:00.000000', 'Ответ на запрос о согласовании чертежей', 'Спасибо за ваш запрос о согласовании чертежей. Мы провели анализ и оценку представленных чертежей и готовы дать согласие на них.'),
        (3, '103Э', '2023-05-10 16:00:00.000000', '2023-05-13 16:00:00.000000', 'Ответ на запрос о проведении совещания', 'В ответ на ваш запрос о проведении совещания, хотим сообщить, что мы назначили дату и время совещания. Подробности будут предоставлены вам в ближайшее время.'),
        (4, '104Э', '2023-01-25 10:30:00.000000', '2023-01-25 10:30:00.000000', 'Ответ на запрос о направлении исходных данных', 'Спасибо за ваш запрос о направлении исходных данных. Мы передаем вам необходимую информацию и готовы ответить на вопросы, которые могут возникнуть.'),
        (5, '105Э', '2023-03-22 14:15:00.000000', '2023-03-19 14:15:00.000000', 'Ответ на запрос о согласовании изменений в проекте', 'В ответ на ваш запрос о согласовании изменений в проекте конструкции, хотим сообщить, что мы проанализировали предложенные изменения и готовы дать свое согласие.'),
        (6, '106Э', '2023-02-10 11:00:00.000000', '2023-02-10 11:00:00.000000', 'Ответ на запрос о разработке детального чертежа', 'Благодарим за ваш запрос о разработке детального чертежа. Наша команда уже начала работу над ним и в ближайшее время предоставит вам результаты.'),
        (7, '107Э', '2023-04-05 09:00:00.000000', '2023-04-05 09:00:00.000000', 'Ответ на запрос о применении новой технологии', 'Спасибо за ваш запрос о применении новой технологии. Мы провели анализ возможностей и готовы приступить к внедрению указанной технологии.'),
        (8, '108Э', '2023-02-18 15:30:00.000000', '2023-02-18 15:30:00.000000', 'Ответ на запрос о разработке прототипа', 'В ответ на ваш запрос о разработке прототипа, рады сообщить, что мы уже начали работу над созданием прототипа и в ближайшее время предоставим вам результаты.'),
        (9, '109Э', '2023-05-20 13:45:00.000000', '2023-05-20 13:45:00.000000', 'Ответ на запрос о проведении испытаний', 'Благодарим за ваш запрос о проведении испытаний. Мы назначили дату и время испытаний и предоставим вам отчет по их результатам.'),
        (10, '110Э', '2023-03-15 11:15:00.000000', '2023-03-15 11:15:00.000000', 'Ответ на запрос о выборе материалов', 'Спасибо за ваш запрос о выборе материалов. Мы провели анализ требований и подобрали подходящие материалы для вашего проекта.'),
        (11, '111Э', '2023-01-30 10:00:00.000000', '2023-01-30 10:00:00.000000', 'Ответ на запрос о технических требованиях', 'В ответ на ваш запрос о технических требованиях, хотим сообщить, что мы изучили предоставленные требования и готовы предоставить необходимую поддержку.'),
        (12, '112Э', '2023-04-08 14:30:00.000000', '2023-04-11 14:30:00.000000', 'Ответ на запрос о технической документации', 'В ответ на ваш запрос о предоставлении технической документации, хотим сообщить, что мы готовы предоставить вам необходимые документы в ближайшее время.'),
        (13, '113Э', '2023-02-05 09:45:00.000000', '2023-02-05 09:45:00.000000', 'Ответ на запрос о разработке электронной схемы', 'Благодарим за ваш запрос о разработке электронной схемы. Мы уже приступили к работе над ней и в скором времени предоставим вам результаты.'),
        (14, '114Э', '2023-05-13 16:00:00.000000', '2023-05-16 16:00:00.000000', 'Ответ на запрос о проведении совещания', 'В ответ на ваш запрос о проведении совещания, хотим сообщить, что мы назначили дату и время совещания. Подробности будут предоставлены вам в ближайшее время.'),
        (15, '115Э', '2023-03-01 11:00:00.000000', '2023-02-25 11:00:00.000000', 'Ответ на запрос о согласовании изменений в проекте', 'В ответ на ваш запрос о согласовании изменений в проекте конструкции, хотим сообщить, что мы проанализировали предложенные изменения и готовы дать свое согласие.'),
        (16, '116Э', '2023-02-15 10:30:00.000000', '2023-02-15 10:30:00.000000', 'Ответ на запрос о разработке детального чертежа', 'Благодарим за ваш запрос о разработке детального чертежа. Наша команда уже начала работу над ним и в ближайшее время предоставит вам результаты.'),
        (17, '117Э', '2023-04-03 09:00:00.000000', '2023-04-03 09:00:00.000000', 'Ответ на запрос о применении новой технологии', 'Спасибо за ваш запрос о применении новой технологии. Мы провели анализ возможностей и готовы приступить к внедрению указанной технологии.'),
        (18, '118Э', '2023-02-20 15:30:00.000000', '2023-02-20 15:30:00.000000', 'Ответ на запрос о разработке прототипа', 'В ответ на ваш запрос о разработке прототипа, рады сообщить, что мы уже начали работу над созданием прототипа и в ближайшее время предоставим вам результаты.'),
        (19, '119Э', '2023-05-18 13:45:00.000000', '2023-05-18 13:45:00.000000', 'Ответ на запрос о проведении испытаний', 'Благодарим за ваш запрос о проведении испытаний. Мы назначили дату и время испытаний и предоставим вам отчет по их результатам.'),
        (20, '120Э', '2023-03-10 11:15:00.000000', '2023-03-10 11:15:00.000000', 'Ответ на запрос о выборе материалов', 'Спасибо за ваш запрос о выборе материалов. Мы провели анализ требований и подобрали подходящие материалы для вашего проекта.');
		COMMIT;