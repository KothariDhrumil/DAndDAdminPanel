import { Component, OnInit } from '@angular/core';
import { ChartConfiguration, ChartData, ChartType } from 'chart.js';
import {
  ApexAxisChartSeries,
  ApexChart,
  ApexXAxis,
  ApexDataLabels,
  ApexTooltip,
  ApexYAxis,
  ApexPlotOptions,
  ApexStroke,
  ApexLegend,
  ApexFill,
  ApexMarkers,
  ApexGrid,
  ApexTitleSubtitle,
  ApexResponsive,
  NgApexchartsModule,
} from 'ng-apexcharts';
import { MatTooltipModule } from '@angular/material/tooltip';
import { BaseChartDirective } from 'ng2-charts';
import { MatProgressBarModule } from '@angular/material/progress-bar';
import { RouterLink } from '@angular/router';
import { MatIconModule } from '@angular/material/icon';
import { MatMenuModule } from '@angular/material/menu';
import { MatButtonModule } from '@angular/material/button';
import { BreadcrumbComponent } from '@shared/components/breadcrumb/breadcrumb.component';
import { MatCardModule } from '@angular/material/card';
import { NgScrollbarModule } from 'ngx-scrollbar';
import { TableCardComponent } from '@shared/components/table-card/table-card.component';
export type ChartOptions = {
  series: ApexAxisChartSeries;
  chart: ApexChart;
  xaxis: ApexXAxis;
  yaxis: ApexYAxis;
  stroke: ApexStroke;
  tooltip: ApexTooltip;
  dataLabels: ApexDataLabels;
  legend: ApexLegend;
  responsive: ApexResponsive[];
  plotOptions: ApexPlotOptions;
  fill: ApexFill;
  colors: string[];
  labels: string[];
  markers: ApexMarkers;
  grid: ApexGrid;
  title: ApexTitleSubtitle;
};

@Component({
  selector: 'app-main',
  templateUrl: './main.component.html',
  styleUrls: ['./main.component.scss'],
  imports: [
    BreadcrumbComponent,
    MatButtonModule,
    MatMenuModule,
    MatCardModule,
    NgScrollbarModule,
    MatIconModule,
    NgApexchartsModule,
    RouterLink,
    MatProgressBarModule,
    BaseChartDirective,
    MatTooltipModule,
    TableCardComponent,
  ],
})
export class MainComponent implements OnInit {
  public areaChartOptions!: Partial<ChartOptions>;
  public barChartOptions!: Partial<ChartOptions>;
  public projectOptions!: Partial<ChartOptions>;
  public smallChart1Options!: Partial<ChartOptions>;
  public smallChart2Options!: Partial<ChartOptions>;
  public smallChart3Options!: Partial<ChartOptions>;
  public smallChart4Options!: Partial<ChartOptions>;
  public performanceRateChartOptions!: Partial<ChartOptions>;

  constructor() {
    // constructor code
  }
  ngOnInit() {
    this.smallChart1();
    this.smallChart2();
    this.smallChart3();
    this.smallChart4();
    this.chart1();
    this.chart2();
    this.chart4();
    this.projectChart();
  }

  // Doughnut chart start

  public doughnutChartOptions: ChartConfiguration['options'] = {
    responsive: true,
    maintainAspectRatio: false,
    plugins: {
      legend: { display: false },
    },
  };
  public doughnutChartLabels: string[] = ['India', 'USA', 'Itely'];
  public doughnutChartData: ChartData<'doughnut'> = {
    labels: this.doughnutChartLabels,
    datasets: [
      {
        data: [350, 450, 100],
        backgroundColor: ['#60A3F6', '#7C59E7', '#DD6811'],
      },
    ],
  };
  public doughnutChartType: ChartType = 'doughnut';

  // Doughnut chart end

  private smallChart1() {
    this.smallChart1Options = {
      series: [
        {
          name: 'Appointments',
          data: [
            50, 61, 80, 50, 72, 52, 60, 41, 30, 45, 70, 40, 93, 63, 50, 62,
          ],
        },
      ],
      chart: {
        height: 90,
        type: 'area',
        toolbar: {
          show: false,
        },
        sparkline: {
          enabled: true,
        },
        foreColor: '#9aa0ac',
      },
      colors: ['#6F42C1'],
      dataLabels: {
        enabled: false,
      },
      stroke: {
        curve: 'smooth',
      },
      xaxis: {
        categories: [
          '16-07-2018',
          '17-07-2018',
          '18-07-2018',
          '19-07-2018',
          '20-07-2018',
          '21-07-2018',
          '22-07-2018',
          '23-07-2018',
          '24-07-2018',
          '25-07-2018',
          '26-07-2018',
          '27-07-2018',
          '28-07-2018',
          '29-07-2018',
          '30-07-2018',
          '31-07-2018',
        ],
      },
      legend: {
        show: false,
      },

      tooltip: {
        theme: 'dark',
        marker: {
          show: true,
        },
        x: {
          show: true,
        },
      },
    };
  }

  private smallChart2() {
    this.smallChart2Options = {
      series: [
        {
          name: 'Operations',
          data: [5, 6, 8, 5, 7, 5, 6, 4, 3, 4, 7, 4, 9, 6, 5, 6],
        },
      ],
      chart: {
        height: 90,
        type: 'area',
        toolbar: {
          show: false,
        },
        sparkline: {
          enabled: true,
        },
        foreColor: '#9aa0ac',
      },
      colors: ['#FD7E14'],
      dataLabels: {
        enabled: false,
      },
      stroke: {
        curve: 'smooth',
      },
      xaxis: {
        categories: [
          '16-07-2018',
          '17-07-2018',
          '18-07-2018',
          '19-07-2018',
          '20-07-2018',
          '21-07-2018',
          '22-07-2018',
          '23-07-2018',
          '24-07-2018',
          '25-07-2018',
          '26-07-2018',
          '27-07-2018',
          '28-07-2018',
          '29-07-2018',
          '30-07-2018',
          '31-07-2018',
        ],
      },
      legend: {
        show: false,
      },

      tooltip: {
        theme: 'dark',
        marker: {
          show: true,
        },
        x: {
          show: true,
        },
      },
    };
  }

  private smallChart3() {
    this.smallChart3Options = {
      series: [
        {
          name: 'New Patients',
          data: [
            50, 61, 80, 50, 72, 52, 60, 41, 30, 45, 70, 40, 93, 63, 50, 62,
          ],
        },
      ],
      chart: {
        height: 90,
        type: 'area',
        toolbar: {
          show: false,
        },
        sparkline: {
          enabled: true,
        },
        foreColor: '#9aa0ac',
      },
      colors: ['#4CAF50'],
      dataLabels: {
        enabled: false,
      },
      stroke: {
        curve: 'smooth',
      },
      xaxis: {
        categories: [
          '16-07-2018',
          '17-07-2018',
          '18-07-2018',
          '19-07-2018',
          '20-07-2018',
          '21-07-2018',
          '22-07-2018',
          '23-07-2018',
          '24-07-2018',
          '25-07-2018',
          '26-07-2018',
          '27-07-2018',
          '28-07-2018',
          '29-07-2018',
          '30-07-2018',
          '31-07-2018',
        ],
      },
      legend: {
        show: false,
      },

      tooltip: {
        theme: 'dark',
        marker: {
          show: true,
        },
        x: {
          show: true,
        },
      },
    };
  }

  private smallChart4() {
    this.smallChart4Options = {
      series: [
        {
          name: 'Earning',
          data: [
            150, 161, 180, 150, 172, 152, 160, 141, 130, 145, 170, 140, 193,
            163, 150, 162,
          ],
        },
      ],
      chart: {
        height: 90,
        type: 'area',
        toolbar: {
          show: false,
        },
        sparkline: {
          enabled: true,
        },
        foreColor: '#9aa0ac',
      },
      colors: ['#2196F3'],
      dataLabels: {
        enabled: false,
      },
      stroke: {
        curve: 'smooth',
      },
      xaxis: {
        categories: [
          '16-07-2018',
          '17-07-2018',
          '18-07-2018',
          '19-07-2018',
          '20-07-2018',
          '21-07-2018',
          '22-07-2018',
          '23-07-2018',
          '24-07-2018',
          '25-07-2018',
          '26-07-2018',
          '27-07-2018',
          '28-07-2018',
          '29-07-2018',
          '30-07-2018',
          '31-07-2018',
        ],
      },
      legend: {
        show: false,
      },

      tooltip: {
        theme: 'dark',
        marker: {
          show: true,
        },
        x: {
          show: true,
        },
      },
    };
  }
  private chart1() {
    this.areaChartOptions = {
      series: [
        {
          name: 'New Clients',
          data: [31, 40, 28, 51, 42, 85, 77],
        },
        {
          name: 'Old Clients',
          data: [11, 32, 45, 32, 34, 52, 41],
        },
      ],
      chart: {
        height: 350,
        type: 'area',
        toolbar: {
          show: false,
        },
        foreColor: '#9aa0ac',
      },
      colors: ['#FC8380', '#6973C6'],
      dataLabels: {
        enabled: false,
      },
      stroke: {
        curve: 'smooth',
      },
      xaxis: {
        type: 'datetime',
        categories: [
          '2018-09-19',
          '2018-09-20',
          '2018-09-21',
          '2018-09-22',
          '2018-09-23',
          '2018-09-24',
          '2018-09-25',
        ],
      },
      legend: {
        show: true,
        position: 'top',
        horizontalAlign: 'center',
        offsetX: 0,
        offsetY: 0,
      },
      grid: {
        show: true,
        borderColor: '#9aa0ac',
        strokeDashArray: 1,
      },
      tooltip: {
        theme: 'dark',
        marker: {
          show: true,
        },
        x: {
          show: true,
        },
      },
    };
  }
  private chart2() {
    this.barChartOptions = {
      series: [
        {
          name: 'New Errors',
          data: [44, 55, 41, 67, 22, 43],
        },
        {
          name: 'Bugs',
          data: [13, 23, 20, 8, 13, 27],
        },
        {
          name: 'Development',
          data: [11, 17, 15, 15, 21, 14],
        },
        {
          name: 'Payment',
          data: [21, 7, 25, 13, 22, 8],
        },
      ],
      chart: {
        type: 'bar',
        height: 350,
        foreColor: '#9aa0ac',
        stacked: true,
        toolbar: {
          show: false,
        },
      },
      responsive: [
        {
          breakpoint: 480,
          options: {
            legend: {
              position: 'bottom',
              offsetX: -10,
              offsetY: 0,
            },
          },
        },
      ],
      plotOptions: {
        bar: {
          horizontal: false,
          columnWidth: '30%',
        },
      },
      dataLabels: {
        enabled: false,
      },
      xaxis: {
        type: 'category',
        categories: ['Mon', 'Tue', 'Wed', 'Thu', 'Fri', 'Sat'],
      },
      grid: {
        show: true,
        borderColor: '#9aa0ac',
        strokeDashArray: 1,
      },
      legend: {
        show: false,
      },
      fill: {
        opacity: 0.8,
        colors: ['#01B8AA', '#374649', '#FD625E', '#F2C80F'],
      },
      tooltip: {
        theme: 'dark',
        marker: {
          show: true,
        },
        x: {
          show: true,
        },
      },
    };
  }

  private projectChart() {
    this.projectOptions = {
      series: [
        {
          name: 'Project A',
          type: 'column',
          data: [23, 11, 22, 27, 13, 22, 37, 21, 44, 22, 30],
        },
        {
          name: 'Project B',
          type: 'area',
          data: [44, 55, 41, 67, 22, 43, 21, 41, 56, 27, 43],
        },
        {
          name: 'Project C',
          type: 'line',
          data: [30, 25, 36, 30, 45, 35, 64, 52, 59, 36, 39],
        },
      ],
      chart: {
        height: 310,
        type: 'line',
        stacked: false,
        foreColor: '#9aa0ac',
      },
      colors: ['#7F7D7F', '#AC93E5', '#FEA861'],
      stroke: {
        width: [0, 2, 5],
        curve: 'smooth',
      },
      plotOptions: {
        bar: {
          columnWidth: '50%',
        },
      },

      fill: {
        opacity: [0.85, 0.25, 1],
        gradient: {
          inverseColors: false,
          shade: 'light',
          type: 'vertical',
          opacityFrom: 0.85,
          opacityTo: 0.55,
        },
      },
      labels: [
        '01/01/2003',
        '02/01/2003',
        '03/01/2003',
        '04/01/2003',
        '05/01/2003',
        '06/01/2003',
        '07/01/2003',
        '08/01/2003',
        '09/01/2003',
        '10/01/2003',
        '11/01/2003',
      ],
      markers: {
        size: 0,
      },
      grid: {
        show: true,
        borderColor: '#9aa0ac',
        strokeDashArray: 1,
      },
      xaxis: {
        type: 'datetime',
      },
      yaxis: {
        title: {
          text: 'Revenue',
        },
        min: 0,
      },
      tooltip: {
        theme: 'dark',
        shared: true,
        intersect: false,
        y: {
          formatter: function (y) {
            if (typeof y !== 'undefined') {
              return y.toFixed(0) + 'k' + ' dollars';
            }
            return y;
          },
        },
      },
    };
  }

  private chart4() {
    this.performanceRateChartOptions = {
      series: [
        {
          name: 'Bill Amount',
          data: [113, 120, 130, 120, 125, 119, 126],
        },
      ],
      chart: {
        height: 350,
        type: 'line',
        dropShadow: {
          enabled: true,
          color: '#000',
          top: 18,
          left: 7,
          blur: 10,
          opacity: 0.2,
        },
        foreColor: '#9aa0ac',
        toolbar: {
          show: false,
        },
      },
      colors: ['#545454'],
      dataLabels: {
        enabled: true,
      },
      stroke: {
        curve: 'smooth',
      },
      markers: {
        size: 1,
      },
      grid: {
        show: true,
        borderColor: '#9aa0ac',
        strokeDashArray: 1,
      },
      xaxis: {
        categories: ['Sun', 'Mon', 'Tue', 'Wed', 'Thu', 'Fri', 'Sat'],
        title: {
          text: 'Weekday',
        },
      },
      yaxis: {
        title: {
          text: 'Bill Amount($)',
        },
      },
      tooltip: {
        theme: 'dark',
        marker: {
          show: true,
        },
        x: {
          show: true,
        },
      },
    };
  }

  invoiceData = [
    {
      invoiceNo: '#IN7865',
      clientName: 'John Doe',
      dueDate: '12/05/2016',
      status: 'Paid',
      total: '$500',
      img: 'assets/images/user/user1.jpg',
      invoiceLink: '#/admin/accounts/invoice',
    },
    {
      invoiceNo: '#IN2301',
      clientName: 'Sarah Smith',
      dueDate: '03/31/2016',
      status: 'Not Paid',
      total: '$372',
      img: 'assets/images/user/user2.jpg',
      invoiceLink: '#/admin/accounts/invoice',
    },
    {
      invoiceNo: '#IN7239',
      clientName: 'Airi Satou',
      dueDate: '04/14/2017',
      status: 'Partially Paid',
      total: '$1038',
      img: 'assets/images/user/user3.jpg',
      invoiceLink: '#/admin/accounts/invoice',
    },
    {
      invoiceNo: '#IN1482',
      clientName: 'Angelica Ramos',
      dueDate: '08/11/2017',
      status: 'Paid',
      total: '$872',
      img: 'assets/images/user/user4.jpg',
      invoiceLink: '#/admin/accounts/invoice',
    },
    {
      invoiceNo: '#IN8526',
      clientName: 'Ashton Cox',
      dueDate: '02/15/2018',
      status: 'Not Paid',
      total: '$2398',
      img: 'assets/images/user/user5.jpg',
      invoiceLink: '#/admin/accounts/invoice',
    },
    {
      invoiceNo: '#IN2473',
      clientName: 'Cara Stevens',
      dueDate: '01/28/2017',
      status: 'Paid',
      total: '$834',
      img: 'assets/images/user/user6.jpg',
      invoiceLink: '#/admin/accounts/invoice',
    },
    {
      invoiceNo: '#IN7366',
      clientName: 'Jacob Ryan',
      dueDate: '03/11/2017',
      status: 'Partially Paid',
      total: '$147',
      img: 'assets/images/user/user7.jpg',
      invoiceLink: '#/admin/accounts/invoice',
    },
    {
      invoiceNo: '#IN5642',
      clientName: 'Emily Walker',
      dueDate: '09/12/2018',
      status: 'Paid',
      total: '$650',
      img: 'assets/images/user/user8.jpg',
      invoiceLink: '#/admin/accounts/invoice',
    },
    {
      invoiceNo: '#IN1457',
      clientName: 'Michael Brown',
      dueDate: '04/20/2019',
      status: 'Not Paid',
      total: '$1220',
      img: 'assets/images/user/user9.jpg',
      invoiceLink: '#/admin/accounts/invoice',
    },
    {
      invoiceNo: '#IN9083',
      clientName: 'Olivia Green',
      dueDate: '10/03/2020',
      status: 'Partially Paid',
      total: '$850',
      img: 'assets/images/user/user10.jpg',
      invoiceLink: '#/admin/accounts/invoice',
    },
    {
      invoiceNo: '#IN3379',
      clientName: 'David Lee',
      dueDate: '06/25/2021',
      status: 'Paid',
      total: '$1295',
      img: 'assets/images/user/user11.jpg',
      invoiceLink: '#/admin/accounts/invoice',
    },
    {
      invoiceNo: '#IN9874',
      clientName: 'Sophia Johnson',
      dueDate: '01/18/2022',
      status: 'Not Paid',
      total: '$320',
      img: 'assets/images/user/user2.jpg',
      invoiceLink: '#/admin/accounts/invoice',
    },
  ];

  invoiceColumnDefinitions = [
    { def: 'invoiceNo', label: 'Invoice No', type: 'text' },
    { def: 'clientName', label: 'Client Name', type: 'text' },
    { def: 'dueDate', label: 'Due Date', type: 'date' },
    { def: 'status', label: 'Status', type: 'badge' },
    { def: 'total', label: 'Total', type: 'currency' },
    { def: 'actions', label: 'Actions', type: 'actionBtn' },
  ];

  // projects

  projectData = [
    {
      projectName: 'Project A',
      employeesTeam: [
        { name: 'John Doe', avatar: 'user1.jpg' },
        { name: 'Jane Smith', avatar: 'user2.jpg' },
        { name: 'Bob Johnson', avatar: 'user3.jpg' },
        { name: 'Alice Williams', avatar: 'user4.jpg' },
      ],
      teamLeader: 'John Doe',
      priority: 'Medium',
      openTask: 19,
      completedTask: 10,
      status: 'Pending',
      documents: 'Contract.pdf',
      actions: 'Edit, Delete',
    },
    {
      projectName: 'Project B',
      employeesTeam: [
        { name: 'Sarah Smith', avatar: 'user7.jpg' },
        { name: 'Michael Johnson', avatar: 'user2.jpg' },
        { name: 'Emily Davis', avatar: 'user8.jpg' },
      ],
      teamLeader: 'Sarah Smith',
      priority: 'Low',
      openTask: 25,
      completedTask: 18,
      status: 'In Progress',
      documents: 'Proposal.pdf',
      actions: 'Edit, Delete',
    },
    {
      projectName: 'Project C',
      employeesTeam: [
        { name: 'Olivia Brown', avatar: 'user9.jpg' },
        { name: 'James Lee', avatar: 'user10.jpg' },
        { name: 'Sophia Wilson', avatar: 'user11.jpg' },
      ],
      teamLeader: 'Olivia Brown',
      priority: 'High',
      openTask: 30,
      completedTask: 25,
      status: 'Completed',
      documents: 'Final_Report.pdf',
      actions: 'Edit, Delete',
    },
    {
      projectName: 'Project D',
      employeesTeam: [
        { name: 'David Martinez', avatar: 'user2.jpg' },
        { name: 'Isabella Taylor', avatar: 'user8.jpg' },
        { name: 'Lucas White', avatar: 'user7.jpg' },
      ],
      teamLeader: 'David Martinez',
      priority: 'Low',
      openTask: 15,
      completedTask: 10,
      status: 'Pending',
      documents: 'Initial_Design.pdf',
      actions: 'Edit, Delete',
    },
    {
      projectName: 'Project E',
      employeesTeam: [
        { name: 'Ethan Green', avatar: 'user5.jpg' },
        { name: 'Mia Clark', avatar: 'user6.jpg' },
        { name: 'Daniel Harris', avatar: 'user9.jpg' },
        { name: 'Charlotte Lewis', avatar: 'user8.jpg' },
      ],
      teamLeader: 'Ethan Green',
      priority: 'Medium',
      openTask: 40,
      completedTask: 30,
      status: 'Completed',
      documents: 'Budget_Sheet.xlsx',
      actions: 'Edit, Delete',
    },
    {
      projectName: 'Project F',
      employeesTeam: [
        { name: 'Jack Robinson', avatar: 'user2.jpg' },
        { name: 'Lily Walker', avatar: 'user1.jpg' },
        { name: 'Henry Adams', avatar: 'user4.jpg' },
      ],
      teamLeader: 'Jack Robinson',
      priority: 'High',
      openTask: 12,
      completedTask: 10,
      status: 'In Progress',
      documents: 'Timeline_GanttChart.xlsx',
      actions: 'Edit, Delete',
    },
    {
      projectName: 'Project G',
      employeesTeam: [
        { name: 'Ava Scott', avatar: 'user2.jpg' },
        { name: 'David Moore', avatar: 'user5.jpg' },
        { name: 'Emma Taylor', avatar: 'user4.jpg' },
        { name: 'Lucas White', avatar: 'user7.jpg' },
        { name: 'Pankaj Patel', avatar: 'user3.jpg' },
      ],
      teamLeader: 'Ava Scott',
      priority: 'Low',
      openTask: 22,
      completedTask: 14,
      status: 'Completed',
      documents: 'Research_Notes.pdf',
      actions: 'Edit, Delete',
    },
    {
      projectName: 'Project H',
      employeesTeam: [
        { name: 'Sophia Miller', avatar: 'user10.jpg' },
        { name: 'Jackson Harris', avatar: 'user11.jpg' },
        { name: 'Ella Clark', avatar: 'user2.jpg' },
      ],
      teamLeader: 'Sophia Miller',
      priority: 'High',
      openTask: 17,
      completedTask: 16,
      status: 'Completed',
      documents: 'User_Guide.pdf',
      actions: 'Edit, Delete',
    },
  ];

  projectColumnDefinitions = [
    { def: 'projectName', label: 'Project Name', type: 'text' },
    { def: 'employeesTeam', label: 'Employee Team', type: 'team' },
    { def: 'teamLeader', label: 'Team Leaders', type: 'text' },
    { def: 'priority', label: 'Priority', type: 'priority' },
    { def: 'openTask', label: 'Open Task', type: 'text' },
    { def: 'completedTask', label: 'Completed Task', type: 'text' },
    { def: 'status', label: 'Status', type: 'text' },
    { def: 'documents', label: 'Documents', type: 'file' },
    { def: 'actions', label: 'Actions', type: 'actionBtn' },
  ];
}
