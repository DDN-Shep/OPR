$(function() {
	'use strict';

	$('[contenteditable]').on('keypress', function(e) {
		$(this).parents('form:first').addClass('changed');
	});

	$('[contenteditable] + footer').on('click', function(e) {
		$(this).siblings('[contenteditable]').focus();
	});

	$('[data-toggle="tooltip"]').tooltip();

	var ch = $.connection.oprHub,
		qs = function() {
			var o = {};

			location.search.substr(1).split('&').forEach(function(i) {
				var s = i.split('='),
					k = s[0],
					v = s[1] && decodeURIComponent(s[1]);

				(k in o) ? o[k].push(v) : o[k] = [v];
			});

			return function(name, index) {
				return (o[name] || [])[index > 0 ? index : 0];
			};
		}(),
		id = qs('id');

	ch.client.id = function(identifier) {
		id = identifier;
	};

	ch.client.loadReport = function(report) {
		function initialiseTable(id, title, report, data, columns) {
			$(id + ' > caption').html(title);

			return $(id).DataTable({
				info: false,
				paging: false,
				data: data,
				columns: columns
			});
		};

		initialiseTable('#data-quality-report',
			[
				'<h4>Data Quality - ', moment(report.DataQualityReport.ReportDate).format('MMM YYYY'), '</h4>'
			].join(''),
			report.DataQualityReport,
			report.DataQualityReport.DataQualities,
			[
				{ data: 'BusinessPlan', title: 'Business Plan' },
				{ data: 'TotalSlips', title: 'Total Slips' },
				{ data: 'SlipsChecked', title: 'Slips Checked' },
				{ data: 'SlipPassRate', title: 'Slip Pass Rate' },
				{ data: 'SoxPassRate', title: 'SOX Pass Rate' },
				{ data: 'FieldErrorRate', title: 'Field Error Rate' },
				{ data: 'SoxFieldErrorRate', title: 'SOX Field Error Rate' }
			]
		);

		initialiseTable('#total-insured-report',
			[
				'<h4>Total Insured Values - ', moment(report.TotalInsuredReport.ReportDate).format('MMM YYYY'), '</h4>'
			].join(''),
			report.TotalInsuredReport,
			report.TotalInsuredReport.TotalInsureds,
			[
				{ data: 'Class', title: 'Class' },
				{ data: 'Total', title: 'Total' },
				{ data: 'TotalChecked', title: 'Total Checked' },
				{ data: 'TotalErrors', title: 'Total Errors' },
				{ data: 'TotalCorrect', title: 'Total Correct' },
				{ data: 'SuccessRate', title: 'Success Rate' }
			]
		);

		initialiseTable('#subscribe-data-entry-report',
			[
				'<h4>Subscribe Data Entry Timeliness - ', moment(report.SubscribeDataEntryReport.ReportDate).format('MMM YYYY'), '</h4> ',
				'SLA: 7 days from Written Date to Date Made Live'
			].join(''),
			report.SubscribeDataEntryReport,
			report.SubscribeDataEntryReport.SubscribeDataEntries,
			[
				{ data: 'BusinessPlan', title: 'Business Plan' },
				{ data: 'CompletedOnTime', title: 'Completed On Time' },
				{ data: 'CompletedLate', title: 'Completed Late' },
				{ data: 'CompletedSuccessRate', title: 'Completed Success Rate' },
				{ data: 'OutstandingOnTime', title: 'Outstanding On Time' },
				{ data: 'OutstandingLate', title: 'Outstanding Late' },
				{ data: 'OutstandingSuccessRate', title: 'Outstanding Success Rate' }//,
				//{ data: 'TotalOnTime', title: 'Total On Time' },
				//{ data: 'TotalLate', title: 'Total Late' },
				//{ data: 'TotalSuccessRate', title: 'Total Success Rate' }
			]
		);
	};

	ch.client.loadComments = function(comments) {
		if (comments) for (var i = 0, c; i < comments.length && (c = comments[i]); i++) {
			ch.client.setComment(c.Type, c.Comment.Author, c.Comment.Comment, c.Comment.CreatedOn);
		}
	};

	ch.client.setComment = function(type, author, comment, date) {
		var $comment = $('#' + type),
			$footer = $comment.siblings('footer');

		$comment.html(comment)
			.data('html', comment)
			.removeClass('empty')
			.parents('form')
			.removeClass('changed');

		$footer.attr('createdby', author).attr('createdon', moment(date).format('DD MMM YYYY'));
	};

	$.connection.hub.start().done(function() {
		if (id) {
			ch.server.getReport(id);
			ch.server.getLatestComments(id);
		}

		$('form').on('submit', function(e) {
			e.preventDefault();

			var type = $(e.target).data('target'),
				comment = $('#' + type, e.target).html();

			ch.server.addComment(id, type, comment);

			e.target.reset();
		});

		$('form > button[name="cancel"]').on('click', function(e) {
			var $comment = $($(e.target).parents('form').data('target'));
			
			$comment.html($comment.data('html'));
		});
	});
});